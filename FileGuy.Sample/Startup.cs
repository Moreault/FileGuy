using System.IO.Compression;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ToolBX.FileGuy;
using ToolBX.FileGuy.Newtonsoft;
using ToolBX.NetAbstractions.Diagnostics;

namespace FileGuy.Sample;

public class Startup : ConsoleStartup
{
    private const string Filepath = "savefile.txt";

    public Startup(IConfiguration configuration) : base(configuration)
    {
    }

    public override void Run(IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var fileSerializer = serviceProvider.GetRequiredService<IFileSerializer>();

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

        console.WriteLine("Saving file...");

        fileSerializer.Serialize(savefile, Filepath, new FileSerializerOptions
        {
            CompressionLevel = CompressionLevel.Fastest,
            DuplicateNameBehavior = DuplicateNameBehavior.Overwrite,
            Serializer = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            }
        });

        console.WriteLine("Decompressing...");
        var loaded = fileSerializer.Decompress<SaveFile>(Filepath);

        console.WriteLine("Save file contents :");
        console.WriteLine(JsonConvert.SerializeObject(loaded, Formatting.Indented));
    }
}