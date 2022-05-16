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
            DummyFile o = null!;
            var filename = Fixture.Create<string>();
            var options = Fixture.Create<FileSerializerOptions>();

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
            var o = Fixture.Create<DummyFile>();
            var options = Fixture.Create<FileSerializerOptions>();

            //Act
            var action = () => Instance.Serialize(o, filename, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_SaveSerializedObject()
        {
            //Arrange
            var o = Fixture.Create<DummyFile>();
            var filename = Fixture.Create<string>();
            var options = Fixture.Create<FileSerializerOptions>();

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
            var options = Fixture.Create<FileSerializerOptions>();

            //Act
            var action = () => Instance.Deserialize<DummyFile>(filename, options);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_DeserializeFileToObject()
        {
            //Arrange
            var filename = Fixture.Create<string>();
            var options = Fixture.Create<FileSerializerOptions>();

            var o = Fixture.Create<DummyFile>();
            var json = JsonSerializer.Serialize(o, options.Serializer);
            GetMock<IFileLoader>().Setup(x => x.LoadAsString(filename)).Returns(json);

            //Act
            var result = Instance.Deserialize<DummyFile>(filename, options);

            //Assert
            result.Should().BeEquivalentTo(o);
        }
    }
}