![FileGuy](https://github.com/Moreault/FileGuy/blob/master/fileguy.png)
# FileGuy.Json

FileGuy.Json is a simple library that allows you to read and write json to and from files in a simple way using the System.Text.Json library.

## FileSerializer
A service that allows you to read and write json to and from files directly.

### Usage
```cs
private readonly FileSerializer _fileSerializer;

public MyClass(FileSerializer fileSerializer)
{
	_fileSerializer = fileSerializer;
}

public void WriteJsonToFile()
{
	var myObject = new MyObject();
	_fileSerializer.Serialize(myObject, "myObject.json");
}
```

A `FileSerializerOptions` object can also be passed which contains a System.Text.Json.JsonSerializerOptions object. This allows you to configure the serialization process.

```cs
var options = new FileSerializerOptions
{
	Serializer = new JsonSerializerOptions
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true
	}
};

_fileSerializer.Serialize(myObject, "myObject.json", options);
```

The `Deserialize` method works as you would expect.

```cs
private readonly FileSerializer _fileSerializer;

public MyClass(FileSerializer fileSerializer)
{
	_fileSerializer = fileSerializer;
}

public void ReadJsonFromFile()
{
	var myObject = _fileSerializer.Deserialize<MyObject>("myObject.json", new FileSerializerOptions
	{
		Serializer = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		}
	});
}
```

### Compression/Decompression
The `FileSerializer` class also has methods for compressing and decompressing json files in the same way that `FileGuy.FileSaver` and `FileGuy.FileLoader` already do.

```cs
private readonly FileSerializer _fileSerializer;

public MyClass(FileSerializer fileSerializer)
{
	_fileSerializer = fileSerializer;
}

public void CompressJsonFile()
{
	_fileSerializer.Serialize("myObject.json", new FileSerializerOptions
	{
		//Uses NoCompression by default
		CompressionLevel = CompressionLevel.Optimal
	});
}

public void DecompressJsonFile()
{
	var myObject = _fileSerializer.Decompress<MyObject>("myObject.json");
	...
}
```