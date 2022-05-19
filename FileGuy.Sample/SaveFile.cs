namespace FileGuy.Sample;

public record SaveFile
{
    public string Name { get; init; } = string.Empty;
    public int Level { get; init; } = 1;
    public int Experience { get; init; }
    public IList<Item> Items { get; init; } = new List<Item>();
}