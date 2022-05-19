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
        var output = _streamFactory.MemoryStream();
        using var compressor = _streamFactory.DeflateStream(output, compressionLevel, true);
        stream.CopyTo(compressor);
        return output;
    }

    public IStream Decompress(IStream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        var output = _streamFactory.MemoryStream();
        using var decompressor = _streamFactory.DeflateStream(stream, CompressionMode.Decompress);
        decompressor.CopyTo(output);
        return output;
    }
}