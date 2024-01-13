namespace ToolBX.FileGuy.Newtonsoft;

[AutoConfig.AutoConfig("DefaultFileSerialization")]
[AutoConfig("DefaultFileSerialization")]
public sealed record FileSerializerOptions : FileSaveOptions
{
    public JsonSerializerSettings Serializer { get; init; } = new();
}