namespace FileGuy.Tests;

[TestClass]
public class FileLoaderTester
{
    [TestClass]
    public class DecompressAsString : Tester<FileLoader>
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenPathIsNullOrEmpty_Throw(string path)
        {
            //Arrange

            //Act
            var action = () => Instance.DecompressAsString(path);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenPathIsValid_ReturnText()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var decompressed = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Decompress(filestream.Object)).Returns(decompressed.Object);

            var text = Fixture.Create<string>();
            var bytes = Encoding.UTF8.GetBytes(text);
            decompressed.Setup(x => x.ToArray()).Returns(bytes);

            //Act
            var result = Instance.DecompressAsString(path);

            //Assert
            result.Should().BeEquivalentTo(text);
        }

        [TestMethod]
        public void Always_DisposeFileStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var decompressed = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Decompress(filestream.Object)).Returns(decompressed.Object);

            var bytes = Fixture.CreateMany<byte>().ToArray();
            decompressed.Setup(x => x.ToArray()).Returns(bytes);

            //Act
            Instance.DecompressAsString(path);

            //Assert
            filestream.Verify(x => x.Dispose());
        }

        [TestMethod]
        public void Always_DisposeDecompressedStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var decompressed = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Decompress(filestream.Object)).Returns(decompressed.Object);

            var bytes = Fixture.CreateMany<byte>().ToArray();
            decompressed.Setup(x => x.ToArray()).Returns(bytes);

            //Act
            Instance.DecompressAsString(path);

            //Assert
            decompressed.Verify(x => x.Dispose());
        }

    }

    [TestClass]
    public class DecompressAsBytes : Tester<FileLoader>
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenPathIsNullOrEmpty_Throw(string path)
        {
            //Arrange

            //Act
            var action = () => Instance.DecompressAsBytes(path);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenPathIsValid_ReturnDecompressedBytes()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var decompressed = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Decompress(filestream.Object)).Returns(decompressed.Object);

            var bytes = Fixture.CreateMany<byte>().ToArray();
            decompressed.Setup(x => x.ToArray()).Returns(bytes);

            //Act
            var result = Instance.DecompressAsBytes(path);

            //Assert
            result.Should().BeEquivalentTo(bytes);
        }

        [TestMethod]
        public void Always_DisposeFileStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var decompressed = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Decompress(filestream.Object)).Returns(decompressed.Object);

            var bytes = Fixture.CreateMany<byte>().ToArray();
            decompressed.Setup(x => x.ToArray()).Returns(bytes);

            //Act
            Instance.DecompressAsBytes(path);

            //Assert
            filestream.Verify(x => x.Dispose());
        }

        [TestMethod]
        public void Always_DisposeMemoryStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var decompressed = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Decompress(filestream.Object)).Returns(decompressed.Object);

            var bytes = Fixture.CreateMany<byte>().ToArray();
            decompressed.Setup(x => x.ToArray()).Returns(bytes);

            //Act
            Instance.DecompressAsBytes(path);

            //Assert
            filestream.Verify(x => x.Dispose());
        }

        [TestMethod]
        public void Always_DisposeDecompressedStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var decompressed = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Decompress(filestream.Object)).Returns(decompressed.Object);

            var bytes = Fixture.CreateMany<byte>().ToArray();
            decompressed.Setup(x => x.ToArray()).Returns(bytes);

            //Act
            Instance.DecompressAsBytes(path);

            //Assert
            decompressed.Verify(x => x.Dispose());
        }
    }

    [TestClass]
    public class DecompressAsStream : Tester<FileLoader>
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenPathIsNullOrEmpty_Throw(string path)
        {
            //Arrange

            //Act
            var action = () => Instance.DecompressAsStream(path);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenPathIsValid_ReturnDecompressedMemoryStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var decompressed = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Decompress(filestream.Object)).Returns(decompressed.Object);

            //Act
            var result = Instance.DecompressAsStream(path);

            //Assert
            result.Should().Be(decompressed.Object);
        }

        [TestMethod]
        public void Always_DisposeFileStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var decompressed = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Decompress(filestream.Object)).Returns(decompressed.Object);

            //Act
            Instance.DecompressAsStream(path);

            //Assert
            filestream.Verify(x => x.Dispose());
        }

        [TestMethod]
        public void Always_DisposeMemoryStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var decompressed = new Mock<IStream>();
            GetMock<IStreamCompressor>().Setup(x => x.Decompress(filestream.Object)).Returns(decompressed.Object);

            //Act
            Instance.DecompressAsStream(path);

            //Assert
            filestream.Verify(x => x.Dispose());
        }
    }

    [TestClass]
    public class LoadAsString : Tester<FileLoader>
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenPathIsNullOrEmpty_Throw(string path)
        {
            //Arrange

            //Act
            var action = () => Instance.LoadAsString(path);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenPathIsValid_ConvertToText()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var text = Fixture.Create<string>();
            var encoded = Encoding.UTF8.GetBytes(text);
            filestream.Setup(x => x.ToArray()).Returns(encoded);

            //Act
            var result = Instance.LoadAsString(path);

            //Assert
            result.Should().BeEquivalentTo(text);
        }

        [TestMethod]
        public void Always_DisposeFileStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var text = Fixture.Create<string>();
            var encoded = Encoding.UTF8.GetBytes(text);
            filestream.Setup(x => x.ToArray()).Returns(encoded);

            //Act
            Instance.LoadAsString(path);

            //Assert
            filestream.Verify(x => x.Dispose());
        }

    }

    [TestClass]
    public class LoadAsBytes : Tester<FileLoader>
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenPathIsNullOrEmpty_Throw(string path)
        {
            //Arrange

            //Act
            var action = () => Instance.LoadAsBytes(path);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenPathIsValid_ConvertToArrayOfBytes()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var bytes = Fixture.CreateMany<byte>().ToArray();
            filestream.Setup(x => x.ToArray()).Returns(bytes);

            //Act
            var result = Instance.LoadAsBytes(path);

            //Assert
            result.Should().BeEquivalentTo(bytes);
        }

        [TestMethod]
        public void Always_DisposeFileStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            var memoryStream = new Mock<IMemoryStream>();
            filestream.Setup(x => x.ToMemoryStream()).Returns(memoryStream.Object);

            var bytes = Fixture.CreateMany<byte>().ToArray();
            memoryStream.As<IStream>().Setup(x => x.ToArray()).Returns(bytes);

            //Act
            Instance.LoadAsBytes(path);

            //Assert
            filestream.Verify(x => x.Dispose());
        }
    }

    [TestClass]
    public class LoadAsStream : Tester<FileLoader>
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenPathIsNullOrEmpty_Throw(string path)
        {
            //Arrange

            //Act
            var action = () => Instance.LoadAsStream(path);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenPathIsValid_ReturnStream()
        {
            //Arrange
            var path = Fixture.Create<string>();

            var filestream = new Mock<IFileStream>();
            GetMock<IStreamFactory>().Setup(x => x.FileStream(path, FileMode.Open)).Returns(filestream.Object);

            //Act
            var result = Instance.LoadAsStream(path);

            //Assert
            result.Should().Be(filestream.Object);
        }
    }
}