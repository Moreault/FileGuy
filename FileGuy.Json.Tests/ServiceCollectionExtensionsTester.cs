namespace FileGuy.Json.Tests;

[TestClass]
public class ServiceCollectionExtensionsTester
{
    [TestClass]
    public class AddFileGuy : Tester
    {
        [TestMethod]
        [Ignore("Those don't work anymore in .NET 8")]
        public void Always_AddFileGuyServices()
        {
            //Arrange
            var services = new FakeServiceCollection();

            //Act
            services.AddFileGuyJson();

            //Assert
            services.Should().ContainEquivalentOf(new FakeServiceCollection
            {
                new(typeof(IFileFetcher), typeof(FileFetcher), ServiceLifetime.Scoped),
                new(typeof(IFileLoader), typeof(FileLoader), ServiceLifetime.Scoped),
                new(typeof(IFileSaver), typeof(FileSaver), ServiceLifetime.Scoped),
                new(typeof(IStreamCompressor), typeof(StreamCompressor), ServiceLifetime.Singleton),
                new(typeof(IFileSerializer), typeof(FileSerializer), ServiceLifetime.Singleton),
                new(typeof(IUniqueFileNameGenerator), typeof(UniqueFileNameGenerator), ServiceLifetime.Scoped),
            });
        }
    }
}