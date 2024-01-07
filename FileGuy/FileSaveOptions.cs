namespace ToolBX.FileGuy;

[AutoConfig.AutoConfig("DefaultFileSaving")]
//TODO 3.0.0 : Remove this one
[AutoConfig("DefaultFileSaving")]
public record FileSaveOptions
{
    public DuplicateNameBehavior DuplicateNameBehavior
    {
        get => _duplicateNameBehavior;
        init => _duplicateNameBehavior = value.ThrowIfUndefined();
    }
    private readonly DuplicateNameBehavior _duplicateNameBehavior = DuplicateNameBehavior.Overwrite;

    public CompressionLevel CompressionLevel
    {
        get => _compressionLevel;
        init => _compressionLevel = value.ThrowIfUndefined();
    }
    private readonly CompressionLevel _compressionLevel = CompressionLevel.NoCompression;
}