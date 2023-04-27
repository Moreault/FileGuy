namespace ToolBX.FileGuy;

public interface IUniqueFileNameGenerator
{
    string Generate(string path);
}

[AutoInject]
public class UniqueFileNameGenerator : IUniqueFileNameGenerator
{
    private readonly IFile _file;
    private readonly IPath _path;

    public UniqueFileNameGenerator(IFile file, IPath path)
    {
        _file = file;
        _path = path;
    }

    public string Generate(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

        var originalPath = path;
        ulong i = 1;

        var directory = _path.GetDirectoryName(originalPath) ?? throw new ArgumentException(string.Format(Exceptions.CannotGenerateUniqueNameBecauseNoRoot, path));
        var filename = _path.GetFileNameWithoutExtension(originalPath);
        var extension = _path.GetExtension(originalPath)!.TrimStart('.');

        while (_file.Exists(path))
            path = _path.Combine(directory, $"{filename} ({i++}).{extension}");

        return path;
    }
}