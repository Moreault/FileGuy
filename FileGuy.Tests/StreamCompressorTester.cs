using ToolBX.FileGuy;

namespace FileGuy.Tests;

[TestClass]
public class StreamCompressorTester
{
    [TestClass]
    public class Compress : Tester<StreamCompressor>
    {
        [TestMethod]
        public void WhenStreamIsNull_Throw()
        {
            //Arrange
            IStream stream = null!;
            var compressionLevel = Fixture.Create<CompressionLevel>();

            //Act
            var action = () => Instance.Compress(stream, compressionLevel);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenCompressionLevelIsNoCompression_Throw()
        {
            //Arrange
            var stream = new Mock<IStream>();
            var compressionLevel = CompressionLevel.NoCompression;

            //Act
            var action = () => Instance.Compress(stream.Object, compressionLevel);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(Exceptions.CannotCompressWithNoCompression);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenCompressionLevelIsValid_CopyInputStreamToDeflate(CompressionLevel compressionLevel)
        {
            //Arrange
            var stream = new Mock<IStream>();

            var output = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream()).Returns(output.Object);

            var compressor = new Mock<IDeflateStream>();
            GetMock<IStreamFactory>().Setup(x => x.DeflateStream(output.Object, compressionLevel, true)).Returns(compressor.Object);

            //Act
            Instance.Compress(stream.Object, compressionLevel);

            //Assert
            stream.Verify(x => x.CopyTo(compressor.Object), Times.Once);
        }

        [TestMethod]
        [DataRow(CompressionLevel.Fastest)]
        [DataRow(CompressionLevel.Optimal)]
        [DataRow(CompressionLevel.SmallestSize)]
        public void WhenCompressionLevelIsValid_ReturnOutputStream(CompressionLevel compressionLevel)
        {
            //Arrange
            var stream = new Mock<IStream>();

            var output = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream()).Returns(output.Object);

            var compressor = new Mock<IDeflateStream>();
            GetMock<IStreamFactory>().Setup(x => x.DeflateStream(output.Object, compressionLevel, true)).Returns(compressor.Object);

            //Act
            var result = Instance.Compress(stream.Object, compressionLevel);

            //Assert
            result.Should().Be(output.Object);
        }
    }

    [TestClass]
    public class Decompress : Tester<StreamCompressor>
    {
        [TestMethod]
        public void WhenStreamIsNull_Throw()
        {
            //Arrange
            IStream stream = null!;

            //Act
            var action = () => Instance.Decompress(stream);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_CopyDeflateToOutput()
        {
            //Arrange
            var stream = new Mock<IStream>();

            var output = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream()).Returns(output.Object);

            var compressor = new Mock<IDeflateStream>();
            GetMock<IStreamFactory>().Setup(x => x.DeflateStream(stream.Object, CompressionMode.Decompress)).Returns(compressor.Object);

            //Act
            Instance.Decompress(stream.Object);

            //Assert
            compressor.Verify(x => x.CopyTo(output.Object), Times.Once);
        }

        [TestMethod]
        public void Always_ReturnUncompressedMemoryStream()
        {
            //Arrange
            var stream = new Mock<IStream>();

            var output = new Mock<IMemoryStream>();
            GetMock<IStreamFactory>().Setup(x => x.MemoryStream()).Returns(output.Object);

            var compressor = new Mock<IDeflateStream>();
            GetMock<IStreamFactory>().Setup(x => x.DeflateStream(stream.Object, CompressionMode.Decompress)).Returns(compressor.Object);

            //Act
            var result = Instance.Decompress(stream.Object);

            //Assert
            result.Should().Be(output.Object);
        }
    }
}