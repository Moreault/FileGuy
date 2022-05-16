namespace FileGuy.Newtonsoft.Tests.Dummies;

public class GenericDummy<T>
{
    public int Id { get; init; }
    public T? Value { get; init; }
}