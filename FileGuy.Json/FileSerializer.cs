namespace ToolBX.FileGuy.Json;

public interface IFileSerializer
{
    void Serialize<T>(T o, string filename, FileSerializerOptions? options = null);
    T Deserialize<T>(string filename, FileSerializerOptions? options = null);
    T Decompress<T>(string filename, FileSerializerOptions? options = null);
}

[AutoInject]
public class FileSerializer : IFileSerializer
{
    private readonly IFileSaver _fileSaver;
    private readonly IFileLoader _fileLoader;

    public FileSerializer(IFileSaver fileSaver, IFileLoader fileLoader)
    {
        _fileSaver = fileSaver;
        _fileLoader = fileLoader;
    }

    public void Serialize<T>(T o, string filename, FileSerializerOptions? options = null)
    {
        if (o == null) throw new ArgumentNullException(nameof(o));
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= new FileSerializerOptions();

        var json = JsonSerializer.Serialize(o, options.Serializer);
        _fileSaver.Save(json, filename, options);
    }

    public T Deserialize<T>(string filename, FileSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= new FileSerializerOptions();
        var json = _fileLoader.LoadAsString(filename);
        return JsonSerializer.Deserialize<T>(json, options.Serializer)!;
    }

    public T Decompress<T>(string filename, FileSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= new FileSerializerOptions();
        var json = _fileLoader.DecompressAsString(filename);
        return JsonSerializer.Deserialize<T>(json, options.Serializer)!;
    }
}