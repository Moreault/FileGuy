namespace FileGuy;

[AutoConfig("DefaultFileSaving")]
public record FileSaveOptions
{
    public DuplicateNameBehavior DuplicateNameBehavior { get; init; } = DuplicateNameBehavior.Overwrite;
    public CompressionLevel CompressionLevel { get; init; } = CompressionLevel.NoCompression;
}