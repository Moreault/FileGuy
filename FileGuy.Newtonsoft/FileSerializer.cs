namespace ToolBX.FileGuy.Newtonsoft;

public interface IFileSerializer
{
    void Serialize<T>(T o, string filename, FileSerializerOptions? options = null);
    T Deserialize<T>(string filename, FileSerializerOptions? options = null);
}

[AutoInject]
public class FileSerializer : IFileSerializer
{
    private readonly IFileSaver _fileSaver;
    private readonly IFileLoader _fileLoader;
    private readonly IJsonConverterFetcher _jsonConverterFetcher;

    public FileSerializer(IFileSaver fileSaver, IFileLoader fileLoader, IJsonConverterFetcher jsonConverterFetcher)
    {
        _fileSaver = fileSaver;
        _fileLoader = fileLoader;
        _jsonConverterFetcher = jsonConverterFetcher;
    }

    public void Serialize<T>(T o, string filename, FileSerializerOptions? options = null)
    {
        if (o == null) throw new ArgumentNullException(nameof(o));
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= new FileSerializerOptions();

        var converters = _jsonConverterFetcher.FetchAll();
        foreach (var converter in converters)
            options.Serializer.Converters.Add(converter);

        var json = JsonConvert.SerializeObject(o, options.Serializer);
        _fileSaver.Save(json, filename, options);
    }

    public T Deserialize<T>(string filename, FileSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= new FileSerializerOptions();

        var json = _fileLoader.LoadAsString(filename);

        var converters = _jsonConverterFetcher.FetchAll();
        foreach (var converter in converters)
            options.Serializer.Converters.Add(converter);

        return JsonConvert.DeserializeObject<T>(json, options.Serializer) ?? throw new Exception($"Could not deserialize file '{filename}'");
    }
}