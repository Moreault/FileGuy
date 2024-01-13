namespace ToolBX.FileGuy;

public interface IFileSaver
{
    void Save(string text, string path, FileSaveOptions? options = null);
    void Save(byte[] file, string path, FileSaveOptions? options = null);
    void Save(IStream stream, string path, FileSaveOptions? options = null);
}

[AutoInject(Lifetime = ServiceLifetime.Scoped)]
public sealed class FileSaver : IFileSaver
{
    private readonly IFile _file;
    private readonly IDirectory _directory;
    private readonly IStreamFactory _streamFactory;
    private readonly IUniqueFileNameGenerator _uniqueFileNameGenerator;
    private readonly IStreamCompressor _streamCompressor;
    private readonly IPath _path;
    private readonly FileSaveOptions _defaultFileSaveOptions;

    public FileSaver(IFile file, IDirectory directory, IStreamFactory streamFactory, IUniqueFileNameGenerator uniqueFileNameGenerator, IStreamCompressor streamCompressor, IPath path, IOptions<FileSaveOptions> options)
    {
        _file = file;
        _directory = directory;
        _streamFactory = streamFactory;
        _uniqueFileNameGenerator = uniqueFileNameGenerator;
        _streamCompressor = streamCompressor;
        _path = path;
        _defaultFileSaveOptions = options.Value ?? new FileSaveOptions();
    }

    public void Save(string text, string path, FileSaveOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
        var bytes = Encoding.UTF8.GetBytes(text);
        Save(bytes, path, options);
    }

    public void Save(byte[] file, string path, FileSaveOptions? options = null)
    {
        if (file == null) throw new ArgumentNullException(nameof(file));
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
        using var stream = _streamFactory.MemoryStream(file);
        Save(stream, path, options);
    }

    public void Save(IStream stream, string path, FileSaveOptions? options = null)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
        options ??= _defaultFileSaveOptions;

        var directory = _path.GetDirectoryName(path)!;

        if (!string.IsNullOrWhiteSpace(directory))
            _directory.EnsureExists(directory);

        if (_file.Exists(path))
        {
            switch (options.DuplicateNameBehavior)
            {
                case DuplicateNameBehavior.Keep:
                    path = _uniqueFileNameGenerator.Generate(path);
                    break;
                case DuplicateNameBehavior.Overwrite:
                    break;
                case DuplicateNameBehavior.Throw:
                    throw new Exception(string.Format(Exceptions.FileAlreadyExists, path));
            }
        }

        if (options.CompressionLevel == CompressionLevel.NoCompression)
        {
            using var filestream = stream.ToFileStream(path);
        }
        else
        {
            using var compressedStream = _streamCompressor.Compress(stream, options.CompressionLevel);
            using var filestream = compressedStream.ToFileStream(path);
        }
    }
}