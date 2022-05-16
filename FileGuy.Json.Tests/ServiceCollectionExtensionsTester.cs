using ToolBX.FileGuy;
using ToolBX.FileGuy.Json;

namespace FileGuy.Json.Tests;

[TestClass]
public class ServiceCollectionExtensionsTester
{
    [TestClass]
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
            services.Should().BeEquivalentTo(new FakeServiceCollection
            {
                new(typeof(IFileLoader), typeof(FileLoader), ServiceLifetime.Singleton),
                new(typeof(IFileSaver), typeof(FileSaver), ServiceLifetime.Singleton),
                new(typeof(IStreamCompressor), typeof(StreamCompressor), ServiceLifetime.Singleton),
                new(typeof(IFileSerializer), typeof(FileSerializer), ServiceLifetime.Singleton),
            });
        }
    }
}