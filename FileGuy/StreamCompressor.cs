namespace ToolBX.FileGuy;

public interface IStreamCompressor
{
    IStream Compress(IStream stream, CompressionLevel compressionLevel = CompressionLevel.Fastest);
    IStream Decompress(IStream stream);
}

[AutoInject]
public class StreamCompressor : IStreamCompressor
{
    private readonly IStreamFactory _streamFactory;

    public StreamCompressor(IStreamFactory streamFactory)
    {
        _streamFactory = streamFactory;
    }

    public IStream Compress(IStream stream, CompressionLevel compressionLevel = CompressionLevel.Fastest)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        if (compressionLevel == CompressionLevel.NoCompression) throw new ArgumentException(Exceptions.CannotCompressWithNoCompression);
        using var compressor = _streamFactory.DeflateStream(stream, compressionLevel);
        return compressor.ToMemoryStream();
    }

    public IStream Decompress(IStream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        using var compressor = _streamFactory.DeflateStream(stream, CompressionMode.Decompress);
        return compressor.ToMemoryStream();
    }
}