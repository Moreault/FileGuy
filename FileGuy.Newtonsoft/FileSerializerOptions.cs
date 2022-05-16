namespace ToolBX.FileGuy.Newtonsoft;

[AutoConfig("DefaultFileSerialization")]
public record FileSerializerOptions : FileSaveOptions
{
    public JsonSerializerSettings Serializer { get; init; } = new();
}