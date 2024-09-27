namespace FileGuy.Newtonsoft.Tests;

[TestClass]
public class JsonConverterFetcherTester
{
    [TestClass]
    public class FetchAll : Tester<JsonConverterFetcher>
    {
        [TestMethod]
        public void Always_ReturnAllSmartConverters()
        {
            //Arrange

            //Act
            var result = Instance.FetchAll();

            //Assert
            result.Select(x => x.GetType()).Should().BeEquivalentTo(new List<Type>
            {
                typeof(GenericGarbageJsonConverter<int>),
                typeof(GenericGarbageJsonConverter<string>),
                typeof(GarbageJsonConverter)
            });
        }
    }
}