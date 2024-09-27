namespace ToolBX.FileGuy.Newtonsoft;

[AutoConfig("DefaultFileSerialization")]
public sealed record FileSerializerOptions : FileSaveOptions
{
    public JsonSerializerSettings Serializer { get; init; } = new();
}