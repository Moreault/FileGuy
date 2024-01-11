namespace ToolBX.FileGuy.Newtonsoft;

public interface IJsonConverterFetcher
{
    IReadOnlyList<JsonConverter> FetchAll();
}

[AutoInject(ServiceLifetime.Singleton)]
public sealed class JsonConverterFetcher : IJsonConverterFetcher
{
    public IReadOnlyList<JsonConverter> FetchAll()
    {
        var smartConverters = Types.Where(x => x.HasAttribute<SmartJsonConverterAttribute>()).ToList();

        var output = new List<JsonConverter>();
        foreach (var type in smartConverters)
        {
            var attribute = type.GetCustomAttribute<SmartJsonConverterAttribute>();
            if (attribute?.Types == null || !attribute.Types.Any())
                output.Add((JsonConverter)Activator.CreateInstance(type)!);
            else
            {
                output.AddRange(attribute.Types.Select(x => (JsonConverter)Activator.CreateInstance(type.MakeGenericType(x))!));
            }
        }

        return output;
    }
}