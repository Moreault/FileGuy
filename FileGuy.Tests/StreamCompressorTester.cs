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
        public void Always_ReturnCompressedMemoryStream(CompressionLevel compressionLevel)
        {
            //Arrange
            var stream = new Mock<IStream>();

            var compressor = new Mock<IDeflateStream>();
            GetMock<IStreamFactory>().Setup(x => x.DeflateStream(stream.Object, compressionLevel)).Returns(compressor.Object);

            var memoryStream = new Mock<IMemoryStream>();
            compressor.Setup(x => x.ToMemoryStream()).Returns(memoryStream.Object);
                
            //Act
            var result = Instance.Compress(stream.Object, compressionLevel);

            //Assert
            result.Should().Be(memoryStream.Object);
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
            var compressionLevel = Fixture.Create<CompressionLevel>();

            //Act
            var action = () => Instance.Decompress(stream);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_ReturnUncompressedMemoryStream()
        {
            //Arrange
            var stream = new Mock<IStream>();

            var compressor = new Mock<IDeflateStream>();
            GetMock<IStreamFactory>().Setup(x => x.DeflateStream(stream.Object, CompressionMode.Decompress)).Returns(compressor.Object);

            var memoryStream = new Mock<IMemoryStream>();
            compressor.Setup(x => x.ToMemoryStream()).Returns(memoryStream.Object);

            //Act
            var result = Instance.Decompress(stream.Object);

            //Assert
            result.Should().Be(memoryStream.Object);
        }
    }
}