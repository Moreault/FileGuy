using System.Text;

namespace FileGuy;

public interface IFileLoader
{
    /// <summary>
    /// Loads and decompressees a file's content as string. Do not use with uncompressed files.
    /// </summary>
    string DecompressAsString(string path);

    /// <summary>
    /// Loads and decompressees a file's content as bytes. Do not use with uncompressed files.
    /// </summary>
    byte[] DecompressAsBytes(string path);

    /// <summary>
    /// Loads and decompressees a file's content as a stream. Do not use with uncompressed files.
    /// </summary>
    IStream DecompressAsStream(string path);

    /// <summary>
    /// Loads an uncompressed file's content as string.
    /// </summary>
    string LoadAsString(string path);

    /// <summary>
    /// Loads an uncompressed file's content as bytes.
    /// </summary>
    byte[] LoadAsBytes(string path);

    /// <summary>
    /// Loads an uncompressed file's content as a stream.
    /// </summary>
    IStream LoadAsStream(string path);
}

[AutoInject]
public class FileLoader : IFileLoader
{
    private readonly IStreamFactory _streamFactory;
    private readonly IStreamCompressor _streamCompressor;

    public FileLoader(IStreamFactory streamFactory, IStreamCompressor streamCompressor)
    {
        _streamFactory = streamFactory;
        _streamCompressor = streamCompressor;
    }

    public string DecompressAsString(string path)
    {
        var bytes = DecompressAsBytes(path);
        return Encoding.UTF8.GetString(bytes);
    }

    public byte[] DecompressAsBytes(string path)
    {
        using var stream = DecompressAsStream(path);
        return stream.ToArray();
    }

    public IStream DecompressAsStream(string path)
    {
        using var stream = LoadAsStream(path);
        return _streamCompressor.Decompress(stream);
    }

    public string LoadAsString(string path)
    {
        var bytes = LoadAsBytes(path);
        return Encoding.UTF8.GetString(bytes);
    }

    public byte[] LoadAsBytes(string path)
    {
        using var stream = LoadAsStream(path);
        return stream.ToArray();
    }

    public IStream LoadAsStream(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
        using var filestream = _streamFactory.FileStream(path, FileMode.Open);
        return filestream.ToMemoryStream();
    }
}