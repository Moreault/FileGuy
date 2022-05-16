namespace FileGuy.Json.Tests;

public record DummyFile
{
    public int Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public IReadOnlyList<Dummy> Data { get; init; } = Array.Empty<Dummy>();
}