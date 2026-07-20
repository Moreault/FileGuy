using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ToolBX.FileGuy.Json;

public interface IFileSerializer
{
    [RequiresUnreferencedCode(FileSerializer.ReflectionRequirementMessage)]
    [RequiresDynamicCode(FileSerializer.ReflectionRequirementMessage)]
    void Serialize<T>(T o, string filename, FileSerializerOptions? options = null);

    /// <summary>
    /// Serializes <paramref name="o"/> to file using source-generated metadata. This overload is trimming and Native AOT safe.
    /// </summary>
    void Serialize<T>(T o, string filename, JsonTypeInfo<T> jsonTypeInfo, FileSaveOptions? options = null);

    [RequiresUnreferencedCode(FileSerializer.ReflectionRequirementMessage)]
    [RequiresDynamicCode(FileSerializer.ReflectionRequirementMessage)]
    T Deserialize<T>(string filename, FileSerializerOptions? options = null);

    /// <summary>
    /// Deserializes a file using source-generated metadata. This overload is trimming and Native AOT safe.
    /// </summary>
    T Deserialize<T>(string filename, JsonTypeInfo<T> jsonTypeInfo);

    [RequiresUnreferencedCode(FileSerializer.ReflectionRequirementMessage)]
    [RequiresDynamicCode(FileSerializer.ReflectionRequirementMessage)]
    T Decompress<T>(string filename, FileSerializerOptions? options = null);

    /// <summary>
    /// Decompresses and deserializes a file using source-generated metadata. This overload is trimming and Native AOT safe.
    /// </summary>
    T Decompress<T>(string filename, JsonTypeInfo<T> jsonTypeInfo);
}

[AutoInject(ServiceLifetime.Scoped)]
public sealed class FileSerializer : IFileSerializer
{
    internal const string ReflectionRequirementMessage =
        "JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use the overload that takes a JsonTypeInfo<T> for trimming and Native AOT support.";

    private readonly IFileSaver _fileSaver;
    private readonly IFileLoader _fileLoader;
    private readonly FileSerializerOptions _defaultFileSerializerOptions;

    public FileSerializer(IFileSaver fileSaver, IFileLoader fileLoader, IOptions<FileSerializerOptions> defaultFileSerializerOptions)
    {
        _fileSaver = fileSaver;
        _fileLoader = fileLoader;
        _defaultFileSerializerOptions = defaultFileSerializerOptions.Value ?? new FileSerializerOptions();
    }

    [RequiresUnreferencedCode(ReflectionRequirementMessage)]
    [RequiresDynamicCode(ReflectionRequirementMessage)]
    public void Serialize<T>(T o, string filename, FileSerializerOptions? options = null)
    {
        if (o == null) throw new ArgumentNullException(nameof(o));
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= _defaultFileSerializerOptions;

        var json = JsonSerializer.Serialize(o, options.Serializer);
        _fileSaver.Save(json, filename, options);
    }

    public void Serialize<T>(T o, string filename, JsonTypeInfo<T> jsonTypeInfo, FileSaveOptions? options = null)
    {
        if (o == null) throw new ArgumentNullException(nameof(o));
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        if (jsonTypeInfo == null) throw new ArgumentNullException(nameof(jsonTypeInfo));
        options ??= _defaultFileSerializerOptions;

        var json = JsonSerializer.Serialize(o, jsonTypeInfo);
        _fileSaver.Save(json, filename, options);
    }

    [RequiresUnreferencedCode(ReflectionRequirementMessage)]
    [RequiresDynamicCode(ReflectionRequirementMessage)]
    public T Deserialize<T>(string filename, FileSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= _defaultFileSerializerOptions;
        var json = _fileLoader.LoadAsString(filename);
        return JsonSerializer.Deserialize<T>(json, options.Serializer)!;
    }

    public T Deserialize<T>(string filename, JsonTypeInfo<T> jsonTypeInfo)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        if (jsonTypeInfo == null) throw new ArgumentNullException(nameof(jsonTypeInfo));
        var json = _fileLoader.LoadAsString(filename);
        return JsonSerializer.Deserialize(json, jsonTypeInfo)!;
    }

    [RequiresUnreferencedCode(ReflectionRequirementMessage)]
    [RequiresDynamicCode(ReflectionRequirementMessage)]
    public T Decompress<T>(string filename, FileSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        options ??= _defaultFileSerializerOptions;
        var json = _fileLoader.DecompressAsString(filename);
        return JsonSerializer.Deserialize<T>(json, options.Serializer)!;
    }

    public T Decompress<T>(string filename, JsonTypeInfo<T> jsonTypeInfo)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(nameof(filename));
        if (jsonTypeInfo == null) throw new ArgumentNullException(nameof(jsonTypeInfo));
        var json = _fileLoader.DecompressAsString(filename);
        return JsonSerializer.Deserialize(json, jsonTypeInfo)!;
    }
}
