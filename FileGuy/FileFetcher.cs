namespace ToolBX.FileGuy;

public interface IFileFetcher
{
    IReadOnlyList<Uri> Fetch(string path, params string[] fileExtensions);
    IReadOnlyList<Uri> Fetch(string path, FileFetchOptions? options = null);
}

[AutoInject(ServiceLifetime.Scoped)]
public sealed class FileFetcher : IFileFetcher
{
    private readonly IDirectory _directory;
    private readonly IPath _path;
    private readonly FileFetchOptions _defaultFileFetchOptions;

    public FileFetcher(IDirectory directory, IPath path, IOptions<FileFetchOptions> defaultFileFetchOptions)
    {
        _directory = directory;
        _path = path;
        _defaultFileFetchOptions = defaultFileFetchOptions.Value;
    }

    public IReadOnlyList<Uri> Fetch(string path, params string[] fileExtensions) => Fetch(path, _defaultFileFetchOptions with
    {
        FileExtensions = fileExtensions
    });

    public IReadOnlyList<Uri> Fetch(string path, FileFetchOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
        options ??= _defaultFileFetchOptions;

        if (!_directory.Exists(path)) return Array.Empty<Uri>();

        var files = _directory.EnumerateFiles(path, "*.*", options.SearchKind);

        if (options.FileExtensions.Any())
            files = files.Where(x => options.FileExtensions.Contains(_path.GetExtensionWithoutDot(x), StringComparer.InvariantCultureIgnoreCase));

        return files.Select(x => new Uri(x, options.UriKind)).ToList();
    }
}