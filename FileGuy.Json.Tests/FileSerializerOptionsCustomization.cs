namespace FileGuy.Json.Tests;

[AutoCustomization]
public class FileSerializerOptionsCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<FileSerializerOptions>(x => x.Without(y => y.Serializer));
    }
}