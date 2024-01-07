![FileGuy](https://github.com/Moreault/FileGuy/blob/master/fileguy.png)
# FileGuy
High-level API for handling files.

## Setup

### With [AutoInject]

If it's not already done, you can add necessary services using in your initialization code :

```cs
services.AddAutoInjectServices();
services.AddAutoConfig();
```

### Without [AutoInject]

Use the following extension method in your initialization code :

```cs
services.AddFileGuy();
```

## FileFetcher
Returns the URIs of files in a directory and/or its subdirectories.

```cs
//This will fetch all files from c:\temp\ regardless of their extension and will not look in subdirectories
var files = _fileFetcher.Fetch("c:\temp\");

//This will only return doc, docx, txt and pdf files from the root of c:\temp\
var files = _fileFetcher.Fetch("c:\temp\", "doc", "docx", "txt", "pdf")

var files _fileFetcher.Fetch("c:\temp\", new FileFetchOptions 
{
	//If omitted, will return all files regardless of extension
	FileExtensions = new List<string> { "pdf", "png" },
	//Will look in all subdirectories as well (default behavior : TopDirectoryOnly)
	SearchKind = SearchOption.AllDirectories,
	//Will return only a relative path to the file (default : Absolute)
	UriKind = UriKind.Relative
});
```

### Default options
There is a way to override default options for your entire project. You need to add the following section to your appsettings.json file. If you don't have an appsettings.json file, then you need to add one to your project.

In the case of console applications, you will have to manually load an appsettings.json file or you can use the AssemblyInitializer.Console package which does this for you.

```json
{
	"DefaultFileFetching": {
		"SearchKind": "AllDirectories",
		"UriKind": "Relative"
	}
}
```

This will ensure that everytime you use `FileFetcher` while omitting options, it will default to `AllDirectories` searches and `Relative` URIs.

## FileLoader
Loads files into memory with or without compression. 

### LoadAsString
Reads an uncompressed file's content and outputs it as a string.

```cs
var file = _fileLoader.LoasAsString("c:/temp/file.txt");
```

### LoadAsBytes
Reads an uncompressed file's content and outputs it as a byte array.

```cs
var file = _fileLoader.LoadAsBytes("c:/temp/file.txt");
```

### LoadAsStream
Loads an uncompressed file in memory as a stream.

```cs
using var file = _fileLoader.LoadAsStream("c:/temp/file.txt");
```

### DecompressAsString
Loads and decompresses a file's content as string.

```cs
using var file = _fileLoader.DecompressAsString("c:/temp/file.zip");
```

### DecompressAsBytes
Loads and decompresses a file's content as a byte array.

```cs
using var file = _fileLoader.DecompressAsBytes("c:/temp/file.zip");
```

### DecompressAsStream
Loads and decompresses a file in memory as a stream.

```cs
using var file = _fileLoader.DecompressAsStream("c:/temp/file.zip");
```

## FileSaver
Saves a file with or without compression while also handling common use cases.

### FileSaveOptions.DuplicateNameBehavior
Dictates what is to be done when a file with the same name exists.

* Overwrite (Default) : Existing file will be overwritten by the new one
* Keep : Existing file will be kept and the new file will have the same name followed by "(x)" (where x is an auto-incremented number)
* Throw : Will throw an exception

### FileSaveOptions.CompressionLevel
Default : No compression

See the official .NET documentation for the [CompressionLevel](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.compressionlevel) enum.

### Save methods
Comes in three flavors :
* `void Save(string text, string path, FileSaveOptions? options = null)`
* `void Save(byte[] file, string path, FileSaveOptions? options = null)`
* `void Save(IStream stream, string path, FileSaveOptions? options = null)`

## StreamCompressor
Compresses and decompresses streams using GZIP.

### Compress

```cs
using var compressedStream = _streamCompressor.Compress(stream);
```

### Decompress

```cs
using var decompressedStream = _streamCompressor.Decompress(stream);
```

## UniqueFileNameGenerator

### Generate
Generates a unique file name based on existing files within a directory to avoid file name collisions.

```cs
//Will return "somephoto.png" or "somephoto (1).png" or "somephoto (2).png" etc... if the file already exists
var fileName = _uniqueFileNameGenerator.Generate("c:/somepath/somephoto.png");
```