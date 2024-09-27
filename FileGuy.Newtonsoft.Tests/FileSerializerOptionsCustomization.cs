namespace FileGuy.Newtonsoft.Tests;

[AutoCustomization]
public class FileSerializerOptionsCustomization : CustomizationBase<FileSerializerOptions>
{
    public override IDummyBuilder<FileSerializerOptions> Build(IDummy dummy) => dummy.Build<FileSerializerOptions>().Without(x => x.Serializer);
}