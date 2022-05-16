![FileGuy](https://github.com/Moreault/FileGuy/blob/master/fileguy.png)
# FileGuy

High-level API for saving and loading files.

## Setup

### With [AutoInject]

Congratulations! You have no additional setup to perform beyond AutoInject's initial setup and can use FileGuy right outside the box after installing the package.

### Without [AutoInject]

Use the following extension method in your initialization code :

```c#
services.AddFileGuy();
```

## Getting started

## Compression

FileGuy doesn't use compression by default but does provides you with the option to compress and decompress your file using gzip's algorithm.

Just look for the CompressionLevel property in FileSaveOptions (or FileSerializerOptions if you're using one of the FileSerializers.)

```c#
var options = new FileSaveOptions { CompressionLevel = CompressionLevel.Optimal };
```

## Default options

Using appsettings.json to setup default file saving behavior for your entire project so you don't have to pass options all the time.

# FileGuy.Json

Adds a FileSerializer service which takes advantage of FileGuy's features to serialize objects to file using Microsoft's System.Text json library.

# FileGuy.Newtonsoft

Similar to FileGuy.Json except that it uses Newtonsoft's json libraries (Json.Net) to achieve serialization.

## Why choose FileGuy.Newtonsoft over FileGuy.Json?

At this point in time, Newtonsoft's json serialization is already part of many already-existing project. It is still going strong as the number one json library for .NET despite Microsoft's attempts to overshadow it with System.Text.

I'm not taking sides here as I believe both are very solid tools. I'm not going to tell you to choose System.Text over Newtonsoft for new projects either since Newtonsoft should still be considered. 

What I'm saying is : Find out for yourself which one works best for you and go with it.

As of 2022, Newtonsoft still has a lot more features but System.Text has more performance. In other words; I don't think either one completely trumps the other and/or renders it obsolete. 

If you still don't know which one to go for then- I don't know- just flip a coin?