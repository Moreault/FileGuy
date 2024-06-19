namespace FileGuy.Newtonsoft.Tests;

[TestClass]
public class ServiceCollectionExtensionsTester
{
    [TestClass]
    [Ignore("Those don't work anymore in .NET 8")]
    public class AddFileGuy : Tester
    {
        [TestMethod]
        public void Always_AddFileGuyServices()
        {
            //Arrange
            var services = new FakeServiceCollection();

            //Act
            services.AddFileGuyJson();

            //Assert
            services.Should().Contain(new FakeServiceCollection
            {
                new(typeof(IFileLoader), typeof(FileLoader), ServiceLifetime.Singleton),
                new(typeof(IFileSaver), typeof(FileSaver), ServiceLifetime.Scoped),
                new(typeof(IStreamCompressor), typeof(StreamCompressor), ServiceLifetime.Singleton),
                new(typeof(IJsonConverterFetcher), typeof(JsonConverterFetcher), ServiceLifetime.Singleton),
                new(typeof(IFileSerializer), typeof(FileSerializer), ServiceLifetime.Singleton),
            });
        }
    }
}