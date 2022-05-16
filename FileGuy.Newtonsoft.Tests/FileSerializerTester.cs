namespace FileGuy.Newtonsoft.Tests;

[TestClass]
public class FileSerializerTester
{
    [TestClass]
    public class Serialize : Tester<FileSerializer>
    {
        protected override void InitializeTest()
        {
            base.InitializeTest();
            GetMock<IJsonConverterFetcher>().Setup(x => x.FetchAll()).Returns(new List<JsonConverter>());
        }

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
        public void WhenJsonConvertersAreFound_UseThemToSerialize()
        {
            //Arrange
            var o = Fixture.Create<DummyFile>();
            var filename = Fixture.Create<string>();
            var options = Fixture.Create<FileSerializerOptions>() with { Serializer = new JsonSerializerSettings { Formatting = Formatting.Indented } };

            var converters = new List<JsonConverter> { new DummyJsonConverter() };
            GetMock<IJsonConverterFetcher>().Setup(x => x.FetchAll()).Returns(converters);

            var jsonOptionsWithConverter = new JsonSerializerSettings { Formatting = Formatting.Indented, Converters = converters };

            var json = JsonConvert.SerializeObject(o, jsonOptionsWithConverter);

            //Act
            Instance.Serialize(o, filename, options);

            //Assert
            GetMock<IFileSaver>().Verify(x => x.Save(json, filename, options));
        }

        [TestMethod]
        public void Always_SaveSerializedObject()
        {
            //Arrange
            var o = Fixture.Create<DummyFile>();
            var filename = Fixture.Create<string>();
            var options = Fixture.Create<FileSerializerOptions>() with { Serializer = new JsonSerializerSettings { Formatting = Formatting.Indented } };

            var json = JsonConvert.SerializeObject(o, options.Serializer);

            //Act
            Instance.Serialize(o, filename, options);

            //Assert
            GetMock<IFileSaver>().Verify(x => x.Save(json, filename, options));
        }
    }

    [TestClass]
    public class Deserialize : Tester<FileSerializer>
    {
        protected override void InitializeTest()
        {
            base.InitializeTest();
            GetMock<IJsonConverterFetcher>().Setup(x => x.FetchAll()).Returns(new List<JsonConverter>());
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenFilenameIsNullOrEmpty_Throw(string filename)
        {
            //Arrange

            //Act
            var action = () => Instance.Deserialize<DummyFile>(filename);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenThereIsAConverter_UseThatConverterToDeserialize()
        {
            //Arrange
            var filename = Fixture.Create<string>();
            var options = Fixture.Create<FileSerializerOptions>() with { Serializer = new JsonSerializerSettings { Formatting = Formatting.Indented } };

            var originalObject = Fixture.Create<DummyFile>();

            var json = JsonConvert.SerializeObject(originalObject, Formatting.Indented, new DummyJsonConverter());
            GetMock<IFileLoader>().Setup(x => x.LoadAsString(filename)).Returns(json);

            var converters = new List<JsonConverter> { new DummyJsonConverter() };
            GetMock<IJsonConverterFetcher>().Setup(x => x.FetchAll()).Returns(converters);

            //Act
            var result = Instance.Deserialize<DummyFile>(filename, options);

            //Assert
            result.Should().BeEquivalentTo(originalObject);
        }

        [TestMethod]
        public void Always_LoadAndDeserializeItem()
        {
            //Arrange
            var filename = Fixture.Create<string>();
            var options = Fixture.Create<FileSerializerOptions>() with { Serializer = new JsonSerializerSettings { Formatting = Formatting.Indented } };

            var originalObject = Fixture.Create<DummyFile>();

            var json = JsonConvert.SerializeObject(originalObject, Formatting.Indented);
            GetMock<IFileLoader>().Setup(x => x.LoadAsString(filename)).Returns(json);

            //Act
            var result = Instance.Deserialize<DummyFile>(filename, options);

            //Assert
            result.Should().BeEquivalentTo(originalObject);
        }
    }
}