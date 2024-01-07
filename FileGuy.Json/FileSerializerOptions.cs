namespace ToolBX.FileGuy.Json;

[AutoConfig.AutoConfig("DefaultFileSerialization")]
//TODO 3.0.0 : remove this one
[AutoConfig("DefaultFileSerialization")]
public sealed record FileSerializerOptions : FileSaveOptions
{
    public JsonSerializerOptions Serializer { get; init; } = new();
}