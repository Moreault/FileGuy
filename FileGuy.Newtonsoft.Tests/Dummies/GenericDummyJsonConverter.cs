namespace FileGuy.Newtonsoft.Tests.Dummies;

[SmartJsonConverter(typeof(int), typeof(string))]
public class GenericDummyJsonConverter<T> : JsonConverter<GenericDummy<T>>
{
    public override void WriteJson(JsonWriter writer, GenericDummy<T>? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override GenericDummy<T>? ReadJson(JsonReader reader, Type objectType, GenericDummy<T>? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}