﻿namespace FileGuy.Tests;

[TestClass]
public class FileSaverTester
{
    [TestClass]
    public class Save_Text : Tester<FileSaver>
    {
        protected override void InitializeTest()
        {
            base.InitializeTest();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(It.IsAny<IStream>(), It.IsAny<CompressionLevel>())).Returns(new Mock<IStream>().Object);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenStreamIsNull_Throw(string text)
        {
            //Arrange
            var path = Dummy.Path.Create();
            var options = Dummy.Create<FileSaveOptions>();

            //Act
            var action = () => Instance.Save(text, path, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenPathIsNullOrEmpty_Throw(string path)
        {
            //Arrange
            var text = Dummy.Create<string>();
            var options = Dummy.Create<FileSaveOptions>();

            //Act
            var action = () => Instance.Save(text, path, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_EnsureDirectoryExists()
        {
            //Arrange
            var text = Dummy.Create<string>();
            var file = Encoding.UTF8.GetBytes(text);
            var path = Dummy.Path.Create();
            var options = Dummy.Create<FileSaveOptions>();

            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(new Mock<IMemoryStream>().Object);

            var directory = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetDirectoryName(path)).Returns(directory);

            //Act
            Instance.Save(text, path, options);

            //Assert
            GetMock<IDirectory>().Verify(x => x.EnsureExists(directory), Times.Once);
        }

        [TestMethod]
        public void WhenFileAlreadyExistsAndBehaviorIsToOverwriteAndNoCompression_SaveFileAsIs()
        {
            //Arrange
            var text = Dummy.Create<string>();
            var file = Encoding.UTF8.GetBytes(text);
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = CompressionLevel.NoCompression, DuplicateNameBehavior = DuplicateNameBehavior.Overwrite };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(text, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenFileAlreadyExistsAndBehaviorIsToOverwriteAndHasCompressionLevel_CompressBeforeSavingAsIs(CompressionLevel compressionLevel)
        {
            //Arrange
            var text = Dummy.Create<string>();
            var file = Encoding.UTF8.GetBytes(text);
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = compressionLevel, DuplicateNameBehavior = DuplicateNameBehavior.Overwrite };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var compressedStream = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(stream.Object, compressionLevel)).Returns(compressedStream.Object);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            compressedStream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(text, path, options);

            //Assert
            compressedStream.Verify(x => x.Dispose(), Times.Once);
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        public void WhenFileAlreadyExistsAndBehaviorIsToThrow_Throw()
        {
            //Arrange
            var text = Dummy.Create<string>();
            var file = Encoding.UTF8.GetBytes(text);
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = Dummy.Create<CompressionLevel>(), DuplicateNameBehavior = DuplicateNameBehavior.Throw };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            //Act
            var action = () => Instance.Save(text, path, options);

            //Assert
            action.Should().Throw<Exception>(string.Format(Exceptions.FileAlreadyExists, path));
        }

        [TestMethod]
        public void WhenFileAlreadyExistsAndBehaviorIsToKeepAndNoCompression_SaveWithUniqueName()
        {
            //Arrange
            var text = Dummy.Create<string>();
            var file = Encoding.UTF8.GetBytes(text);
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = CompressionLevel.NoCompression, DuplicateNameBehavior = DuplicateNameBehavior.Keep };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var uniqueName = Dummy.Path.Create();
            GetMock<IUniqueFileNameGenerator>().Setup(x => x.Generate(path)).Returns(uniqueName);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(uniqueName)).Returns(filestream.Object);

            //Act
            Instance.Save(text, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenFileAlreadyExistsAndBehaviorIsToKeepAndHasCompressionLevel_CompressBeforeSavingWithUniqueName(CompressionLevel compressionLevel)
        {
            //Arrange
            var text = Dummy.Create<string>();
            var file = Encoding.UTF8.GetBytes(text);
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = compressionLevel, DuplicateNameBehavior = DuplicateNameBehavior.Keep };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var uniqueName = Dummy.Path.Create();
            GetMock<IUniqueFileNameGenerator>().Setup(x => x.Generate(path)).Returns(uniqueName);

            var compressedStream = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(stream.Object, compressionLevel)).Returns(compressedStream.Object);

            var filestream = new Mock<IFileStream>();
            compressedStream.Setup(x => x.ToFileStream(uniqueName)).Returns(filestream.Object);

            //Act
            Instance.Save(text, path, options);

            //Assert
            compressedStream.Verify(x => x.Dispose(), Times.Once);
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        public void WhenFileDoesNotExistAndNoCompression_SaveFileAsIs()
        {
            //Arrange
            var text = Dummy.Create<string>();
            var file = Encoding.UTF8.GetBytes(text);
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = CompressionLevel.NoCompression, DuplicateNameBehavior = Dummy.Create<DuplicateNameBehavior>() };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(false);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(text, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenFileDoesNotExistAndHasCompressionLevel_CompressBeforeSavingFile(CompressionLevel compressionLevel)
        {
            //Arrange
            var text = Dummy.Create<string>();
            var file = Encoding.UTF8.GetBytes(text);
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = compressionLevel, DuplicateNameBehavior = Dummy.Create<DuplicateNameBehavior>() };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(false);

            var compressedStream = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(stream.Object, compressionLevel)).Returns(compressedStream.Object);

            var filestream = new Mock<IFileStream>();
            compressedStream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(text, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }
    }

    [TestClass]
    public class Save_Bytes : Tester<FileSaver>
    {
        protected override void InitializeTest()
        {
            base.InitializeTest();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(It.IsAny<IStream>(), It.IsAny<CompressionLevel>())).Returns(new Mock<IStream>().Object);
        }

        [TestMethod]
        public void WhenStreamIsNull_Throw()
        {
            //Arrange
            byte[] file = null!;
            var path = Dummy.Path.Create();
            var options = Dummy.Create<FileSaveOptions>();

            //Act
            var action = () => Instance.Save(file, path, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenPathIsNullOrEmpty_Throw(string path)
        {
            //Arrange
            var file = Dummy.Create<byte[]>();
            var options = Dummy.Create<FileSaveOptions>();

            //Act
            var action = () => Instance.Save(file, path, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_EnsureDirectoryExists()
        {
            //Arrange
            var file = Dummy.Create<byte[]>();
            var path = Dummy.Path.Create();
            var options = Dummy.Create<FileSaveOptions>();

            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(new Mock<IMemoryStream>().Object);

            var directory = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetDirectoryName(path)).Returns(directory);

            //Act
            Instance.Save(file, path, options);

            //Assert
            GetMock<IDirectory>().Verify(x => x.EnsureExists(directory), Times.Once);
        }

        [TestMethod]
        public void WhenFileAlreadyExistsAndBehaviorIsToOverwriteAndNoCompression_SaveFileAsIs()
        {
            //Arrange
            var file = Dummy.Create<byte[]>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = CompressionLevel.NoCompression, DuplicateNameBehavior = DuplicateNameBehavior.Overwrite };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(file, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenFileAlreadyExistsAndBehaviorIsToOverwriteAndHasCompressionLevel_CompressBeforeSavingAsIs(CompressionLevel compressionLevel)
        {
            //Arrange
            var file = Dummy.Create<byte[]>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = compressionLevel, DuplicateNameBehavior = DuplicateNameBehavior.Overwrite };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var compressedStream = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(stream.Object, compressionLevel)).Returns(compressedStream.Object);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            compressedStream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(file, path, options);

            //Assert
            compressedStream.Verify(x => x.Dispose(), Times.Once);
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        public void WhenFileAlreadyExistsAndBehaviorIsToThrow_Throw()
        {
            //Arrange
            var file = Dummy.Create<byte[]>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = Dummy.Create<CompressionLevel>(), DuplicateNameBehavior = DuplicateNameBehavior.Throw };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            //Act
            var action = () => Instance.Save(file, path, options);

            //Assert
            action.Should().Throw<Exception>(string.Format(Exceptions.FileAlreadyExists, path));
        }

        [TestMethod]
        public void WhenFileAlreadyExistsAndBehaviorIsToKeepAndNoCompression_SaveWithUniqueName()
        {
            //Arrange
            var file = Dummy.Create<byte[]>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = CompressionLevel.NoCompression, DuplicateNameBehavior = DuplicateNameBehavior.Keep };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var uniqueName = Dummy.Path.Create();
            GetMock<IUniqueFileNameGenerator>().Setup(x => x.Generate(path)).Returns(uniqueName);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(uniqueName)).Returns(filestream.Object);

            //Act
            Instance.Save(file, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenFileAlreadyExistsAndBehaviorIsToKeepAndHasCompressionLevel_CompressBeforeSavingWithUniqueName(CompressionLevel compressionLevel)
        {
            //Arrange
            var file = Dummy.Create<byte[]>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = compressionLevel, DuplicateNameBehavior = DuplicateNameBehavior.Keep };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var uniqueName = Dummy.Path.Create();
            GetMock<IUniqueFileNameGenerator>().Setup(x => x.Generate(path)).Returns(uniqueName);

            var compressedStream = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(stream.Object, compressionLevel)).Returns(compressedStream.Object);

            var filestream = new Mock<IFileStream>();
            compressedStream.Setup(x => x.ToFileStream(uniqueName)).Returns(filestream.Object);

            //Act
            Instance.Save(file, path, options);

            //Assert
            compressedStream.Verify(x => x.Dispose(), Times.Once);
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        public void WhenFileDoesNotExistAndNoCompression_SaveFileAsIs()
        {
            //Arrange
            var file = Dummy.Create<byte[]>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = CompressionLevel.NoCompression, DuplicateNameBehavior = Dummy.Create<DuplicateNameBehavior>() };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(false);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(file, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenFileDoesNotExistAndHasCompressionLevel_CompressBeforeSavingFile(CompressionLevel compressionLevel)
        {
            //Arrange
            var file = Dummy.Create<byte[]>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = compressionLevel, DuplicateNameBehavior = Dummy.Create<DuplicateNameBehavior>() };

            var stream = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream(file)).Returns(stream.Object);

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(false);

            var compressedStream = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(stream.Object, compressionLevel)).Returns(compressedStream.Object);

            var filestream = new Mock<IFileStream>();
            compressedStream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(file, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }
    }

    [TestClass]
    public class Save_Stream : Tester<FileSaver>
    {
        protected override void InitializeTest()
        {
            base.InitializeTest();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(It.IsAny<IStream>(), It.IsAny<CompressionLevel>())).Returns(new Mock<IStream>().Object);
        }

        [TestMethod]
        public void WhenStreamIsNull_Throw()
        {
            //Arrange
            IStream stream = null!;
            var path = Dummy.Path.Create();
            var options = Dummy.Create<FileSaveOptions>();

            //Act
            var action = () => Instance.Save(stream, path, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenPathIsNullOrEmpty_Throw(string path)
        {
            //Arrange
            var stream = new Mock<IStream>();
            var options = Dummy.Create<FileSaveOptions>();

            //Act
            var action = () => Instance.Save(stream.Object, path, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_EnsureDirectoryExists()
        {
            //Arrange
            var stream = new Mock<IStream>();
            var path = Dummy.Path.Create();
            var options = Dummy.Create<FileSaveOptions>();

            var directory = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetDirectoryName(path)).Returns(directory);

            //Act
            Instance.Save(stream.Object, path, options);

            //Assert
            GetMock<IDirectory>().Verify(x => x.EnsureExists(directory), Times.Once);
        }

        [TestMethod]
        public void WhenFileAlreadyExistsAndBehaviorIsToOverwriteAndNoCompression_SaveFileAsIs()
        {
            //Arrange
            var stream = new Mock<IStream>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = CompressionLevel.NoCompression, DuplicateNameBehavior = DuplicateNameBehavior.Overwrite };

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(stream.Object, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenFileAlreadyExistsAndBehaviorIsToOverwriteAndHasCompressionLevel_CompressBeforeSavingAsIs(CompressionLevel compressionLevel)
        {
            //Arrange
            var stream = new Mock<IStream>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = compressionLevel, DuplicateNameBehavior = DuplicateNameBehavior.Overwrite };

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var compressedStream = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(stream.Object, compressionLevel)).Returns(compressedStream.Object);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            compressedStream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(stream.Object, path, options);

            //Assert
            compressedStream.Verify(x => x.Dispose(), Times.Once);
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        public void WhenFileAlreadyExistsAndBehaviorIsToThrow_Throw()
        {
            //Arrange
            var stream = new Mock<IStream>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = Dummy.Create<CompressionLevel>(), DuplicateNameBehavior = DuplicateNameBehavior.Throw };

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            //Act
            var action = () => Instance.Save(stream.Object, path, options);

            //Assert
            action.Should().Throw<Exception>(string.Format(Exceptions.FileAlreadyExists, path));
        }

        [TestMethod]
        public void WhenFileAlreadyExistsAndBehaviorIsToKeepAndNoCompression_SaveWithUniqueName()
        {
            //Arrange
            var stream = new Mock<IStream>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = CompressionLevel.NoCompression, DuplicateNameBehavior = DuplicateNameBehavior.Keep };

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var uniqueName = Dummy.Path.Create();
            GetMock<IUniqueFileNameGenerator>().Setup(x => x.Generate(path)).Returns(uniqueName);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(uniqueName)).Returns(filestream.Object);

            //Act
            Instance.Save(stream.Object, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenFileAlreadyExistsAndBehaviorIsToKeepAndHasCompressionLevel_CompressBeforeSavingWithUniqueName(CompressionLevel compressionLevel)
        {
            //Arrange
            var stream = new Mock<IStream>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = compressionLevel, DuplicateNameBehavior = DuplicateNameBehavior.Keep };

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var uniqueName = Dummy.Path.Create();
            GetMock<IUniqueFileNameGenerator>().Setup(x => x.Generate(path)).Returns(uniqueName);

            var compressedStream = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(stream.Object, compressionLevel)).Returns(compressedStream.Object);

            var filestream = new Mock<IFileStream>();
            compressedStream.Setup(x => x.ToFileStream(uniqueName)).Returns(filestream.Object);

            //Act
            Instance.Save(stream.Object, path, options);

            //Assert
            compressedStream.Verify(x => x.Dispose(), Times.Once);
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        public void WhenFileDoesNotExistAndNoCompression_SaveFileAsIs()
        {
            //Arrange
            var stream = new Mock<IStream>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = CompressionLevel.NoCompression, DuplicateNameBehavior = Dummy.Create<DuplicateNameBehavior>() };

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(false);

            var filestream = new Mock<IFileStream>();
            stream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(stream.Object, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenFileDoesNotExistAndHasCompressionLevel_CompressBeforeSavingFile(CompressionLevel compressionLevel)
        {
            //Arrange
            var stream = new Mock<IStream>();
            var path = Dummy.Path.Create();
            var options = new FileSaveOptions { CompressionLevel = compressionLevel, DuplicateNameBehavior = Dummy.Create<DuplicateNameBehavior>() };

            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(false);

            var compressedStream = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Compress(stream.Object, compressionLevel)).Returns(compressedStream.Object);

            var filestream = new Mock<IFileStream>();
            compressedStream.Setup(x => x.ToFileStream(path)).Returns(filestream.Object);

            //Act
            Instance.Save(stream.Object, path, options);

            //Assert
            filestream.Verify(x => x.Dispose(), Times.Once);
        }
    }
}