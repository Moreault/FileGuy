namespace FileGuy.Json.Tests;

[TestClass]
public class FileSerializerTester
{
    [TestClass]
    public class Serialize : Tester<FileSerializer>
    {
        [TestMethod]
        public void WhenObjectIsNull_Throw()
        {
            //Arrange
            GarbageFile o = null!;
            var filename = Dummy.Create<string>();
            var options = Dummy.Create<FileSerializerOptions>();

            //Act
            var action = () => Instance.Serialize(o, filename, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenFilenameIsNullOrEmpty_Throw(string filename)
        {
            //Arrange
            var o = Dummy.Create<GarbageFile>();
            var options = Dummy.Create<FileSerializerOptions>();

            //Act
            var action = () => Instance.Serialize(o, filename, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_SaveSerializedObject()
        {
            //Arrange
            var o = Dummy.Create<GarbageFile>();
            var filename = Dummy.Create<string>();
            var options = Dummy.Create<FileSerializerOptions>();

            var json = JsonSerializer.Serialize(o, options.Serializer);

            //Act
            Instance.Serialize(o, filename, options);

            //Assert
            GetMock<IFileSaver>().Verify(x => x.Save(json, filename, options), Times.Once);
        }
    }

    [TestClass]
    public class Deserialize : Tester<FileSerializer>
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenFilenameIsNullOrEmpty_Throw(string filename)
        {
            //Arrange
            var options = Dummy.Create<FileSerializerOptions>();

            //Act
            var action = () => Instance.Deserialize<GarbageFile>(filename, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_DeserializeFileToObject()
        {
            //Arrange
            var filename = Dummy.Create<string>();
            var options = Dummy.Create<FileSerializerOptions>();

            var o = Dummy.Create<GarbageFile>();
            var json = JsonSerializer.Serialize(o, options.Serializer);
            GetMock<IFileLoader>().Setup(x => x.LoadAsString(filename)).Returns(json);

            //Act
            var result = Instance.Deserialize<GarbageFile>(filename, options);

            //Assert
            result.Should().BeEquivalentTo(o);
        }
    }

    [TestClass]
    public class Decompress : Tester<FileSerializer>
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenFilenameIsNullOrEmpty_Throw(string filename)
        {
            //Arrange
            var options = Dummy.Create<FileSerializerOptions>();

            //Act
            var action = () => Instance.Decompress<GarbageFile>(filename, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_DecompressAndDeserializeFileToObject()
        {
            //Arrange
            var filename = Dummy.Create<string>();
            var options = Dummy.Create<FileSerializerOptions>();

            var o = Dummy.Create<GarbageFile>();
            var json = JsonSerializer.Serialize(o, options.Serializer);
            GetMock<IFileLoader>().Setup(x => x.DecompressAsString(filename)).Returns(json);

            //Act
            var result = Instance.Decompress<GarbageFile>(filename, options);

            //Assert
            result.Should().BeEquivalentTo(o);
        }
    }
}