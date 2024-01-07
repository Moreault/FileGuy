namespace FileGuy.Sample;

public interface ISample
{
    void Start();
}

[AutoInject]
public class Sample : ISample
{
    private const string Filepath = "savefile.txt";

    private readonly ITerminal _terminal;
    private readonly IFileSerializer _fileSerializer;

    public Sample(ITerminal terminal, IFileSerializer fileSerializer)
    {
        _terminal = terminal;
        _fileSerializer = fileSerializer;
    }
    //TODO Test (a lot) more cases : appsettings default options, both Newtonsoft AND Microsoft serialization, FileFetcher, FileLoader, FileSaver...
    public void Start()
    {
        var savefile = new SaveFile
        {
            Name = "Tata",
            Level = 24,
            Experience = 95012,
            Items = new List<Item>
            {
                new(){Name = "Starfinder"},
                new(){Name = "Potion"},
                new(){Name = "Serum"},
            }
        };

        _terminal.Write("Saving file...");

        _fileSerializer.Serialize(savefile, Filepath, new FileSerializerOptions
        {
            CompressionLevel = CompressionLevel.Fastest,
            DuplicateNameBehavior = DuplicateNameBehavior.Overwrite,
            Serializer = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            }
        });

        _terminal.Write("Decompressing...");
        var loaded = _fileSerializer.Decompress<SaveFile>(Filepath);

        _terminal.Write("Save file contents :");
        _terminal.Write(JsonConvert.SerializeObject(loaded, Formatting.Indented));
    }
}