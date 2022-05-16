namespace FileGuy.Json;

[AutoConfig("DefaultFileSerialization")]
public record FileSerializerOptions : FileSaveOptions
{
    public JsonSerializerOptions Serializer { get; init; } = new();
}