namespace FileGuy.Newtonsoft.Tests.Dummies;

[SmartJsonConverter(typeof(int), typeof(string))]
public class GenericGarbageJsonConverter<T> : JsonConverter<GenericGarbage<T>>
{
    public override void WriteJson(JsonWriter writer, GenericGarbage<T>? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override GenericGarbage<T>? ReadJson(JsonReader reader, Type objectType, GenericGarbage<T>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}