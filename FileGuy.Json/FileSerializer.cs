using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ToolBX.FileGuy.Json;

public interface IFileSerializer
{
    void Serialize<T>(T o, string filename, FileSerializerOptions? options = null);
    T Deserialize<T>(string filename, FileSerializerOptions? options = null);
    T Decompress<T>(string filename, FileSerializerOptions? options = null);
}

[AutoInject(ServiceLifetime.Scoped)]
public sealed class FileSerializer : IFileSerializer
{
    private readonly IFileSaver _fileSaver;
    private readonly IFileLoader _fileLoader;
    private readonly FileSerializerOptions _defaultFileSerializerOptions;

    public FileSerializer(IFileSaver fileSaver, IFileLoader fileLoader, IOptions<FileSerializerOptions> defaultFileSerializerOptions)
    {
        _fileSaver = fileSaver;
        _fileLoader = fileLoader;
        _defaultFileSerializerOptions = defaultFileSerializerOptions.Value ?? new FileSerializerOptions();
    }

    public void Serialize<T>(T o, string filename, FileSerializerOptions? options = null)
    {
        if (o == null) throw new ArgumentNullException(nameof(o));
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= _defaultFileSerializerOptions;

        var json = JsonSerializer.Serialize(o, options.Serializer);
        _fileSaver.Save(json, filename, options);
    }

    public T Deserialize<T>(string filename, FileSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= _defaultFileSerializerOptions;
        var json = _fileLoader.LoadAsString(filename);
        return JsonSerializer.Deserialize<T>(json, options.Serializer)!;
    }

    public T Decompress<T>(string filename, FileSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= _defaultFileSerializerOptions;
        var json = _fileLoader.DecompressAsString(filename);
        return JsonSerializer.Deserialize<T>(json, options.Serializer)!;
    }
}