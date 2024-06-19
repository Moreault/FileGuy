using ToolBX.EasyTypeParsing;

namespace FileGuy.Newtonsoft.Tests.Dummies;

[SmartJsonConverter]
internal class GarbageJsonConverter : JsonConverter<Garbage>
{
    public override Garbage? ReadJson(JsonReader reader, Type objectType, Garbage? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = (string)reader.Value!;
        value = value.Trim('{', '}');
        var splitted = value.Split(',').Select(x => x.Split('=').Select(y => y.Trim()).ToArray()[1]).ToList();

        return new Garbage
        {
            Id = splitted[0].ToIntOrThrow(),
            Precision = splitted[1].ToFloatOrThrow(),
            Value = splitted[2]
        };
    }

    public override void WriteJson(JsonWriter writer, Garbage? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString());
    }
}