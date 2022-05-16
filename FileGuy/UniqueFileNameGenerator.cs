namespace FileGuy;

public interface IUniqueFileNameGenerator
{
    string Generate(string path);
}

[AutoInject]
public class UniqueFileNameGenerator : IUniqueFileNameGenerator
{
    private readonly IFile _file;

    public UniqueFileNameGenerator(IFile file)
    {
        _file = file;
    }

    public string Generate(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

        var originalPath = path;
        ulong i = 1;

        var directory = Path.GetDirectoryName(originalPath) ?? throw new ArgumentException(string.Format(Exceptions.CannotGenerateUniqueNameBecauseNoRoot, path));
        var filename = Path.GetFileNameWithoutExtension(originalPath);
        var extension = Path.GetExtension(originalPath).TrimStart('.');

        while (_file.Exists(path))
            path = Path.Combine(directory, $"{filename} ({i++}).{extension}");

        return path;
    }
}