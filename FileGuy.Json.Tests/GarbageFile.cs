namespace FileGuy.Json.Tests;

public record GarbageFile
{
    public int Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public IReadOnlyList<Garbage> Data { get; init; } = Array.Empty<Garbage>();
}