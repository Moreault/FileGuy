namespace ToolBX.FileGuy.Json;

[AutoConfig("DefaultFileSerialization")]
public sealed record FileSerializerOptions : FileSaveOptions
{
    public JsonSerializerOptions Serializer { get; init; } = new();
}