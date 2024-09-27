namespace FileGuy.Newtonsoft.Tests.Dummies;

public record Garbage
{
    public int Id { get; init; }
    public float Precision { get; init; }
    public string Value { get; init; } = string.Empty;
}