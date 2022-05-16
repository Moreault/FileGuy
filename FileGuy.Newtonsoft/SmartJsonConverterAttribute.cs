namespace FileGuy.Newtonsoft;

/// <summary>
/// Automatically adds Newtonsoft JSON converters that use generic arguments.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SmartJsonConverterAttribute : Attribute
{
    public Type[] Types { get; init; }

    public SmartJsonConverterAttribute(params Type[] types)
    {
        Types = types;
    }
}