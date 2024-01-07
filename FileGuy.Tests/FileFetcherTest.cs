namespace FileGuy.Tests;

[TestClass]
public class FileFetcherTest : Tester<FileFetcher>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        GetMock<IOptions<FileFetchOptions>>().Setup(x => x.Value).Returns(new FileFetchOptions());
        Fixture.Register(() => new[] { UriKind.Absolute, UriKind.Relative });
    }

    [TestMethod]
    public void FetchWithFileExtensions_WhenPathIsNull_Throw() => Ensure.WhenIsNullOrWhiteSpace(path =>
    {
        //Arrange
        var fileExtensions = Fixture.CreateMany<string>().ToArray();

        //Act
        var action = () => Instance.Fetch(path, fileExtensions);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(path));
    });

    [TestMethod]
    public void FetchWithFileExtensions_WhenDirectoryDoesNotExist_ReturnEmpty()
    {
        //Arrange
        var path = Fixture.Create<string>();
        var fileExtensions = Fixture.CreateMany<string>().ToArray();

        GetMock<IDirectory>().Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
        GetMock<IDirectory>().Setup(x => x.Exists(path)).Returns(false);

        //Act
        var result = Instance.Fetch(path, fileExtensions);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void FetchWithFileExtensions_WhenDirectoryExistsAndFileExtensionsEmpty_ReturnAllFiles()
    {
        //Arrange
        var defaultOptions = Fixture.Create<FileFetchOptions>();
        GetMock<IOptions<FileFetchOptions>>().Setup(x => x.Value).Returns(defaultOptions);

        var path = Fixture.Create<string>();
        var fileExtensions = Array.Empty<string>();

        GetMock<IDirectory>().Setup(x => x.Exists(path)).Returns(true);

        var files = Fixture.CreateMany<string>().ToList();
        GetMock<IDirectory>().Setup(x => x.EnumerateFiles(path, "*.*", defaultOptions.SearchKind)).Returns(files);

        //Act
        var result = Instance.Fetch(path, fileExtensions);

        //Assert
        result.Should().BeEquivalentTo(files.Select(x => new Uri(x, defaultOptions.UriKind)));
    }

    [TestMethod]
    public void FetchWithFileExtensions_WhenDirectoryExistsAndFileExtensionsNotEmpty_ReturnOnlyFilesWithExtensions()
    {
        //Arrange
        var defaultOptions = Fixture.Create<FileFetchOptions>();
        GetMock<IOptions<FileFetchOptions>>().Setup(x => x.Value).Returns(defaultOptions);

        var path = Fixture.Create<string>();
        var fileExtensions = Fixture.CreateMany<string>().ToArray();

        GetMock<IDirectory>().Setup(x => x.Exists(path)).Returns(true);

        var files = Fixture.CreateMany<string>().ToList();
        GetMock<IDirectory>().Setup(x => x.EnumerateFiles(path, "*.*", defaultOptions.SearchKind)).Returns(files);

        var filesWithExtension = Fixture.CreateMany<string>().ToList();
        foreach (var file in filesWithExtension)
            GetMock<IPath>().Setup(x => x.GetExtensionWithoutDot(file)).Returns(fileExtensions.GetRandom());

        files.AddRange(filesWithExtension);

        //Act
        var result = Instance.Fetch(path, fileExtensions);

        //Assert
        result.Should().BeEquivalentTo(filesWithExtension.Select(x => new Uri(x, defaultOptions.UriKind)));
    }

    [TestMethod]
    public void FetchWithOptions_WhenPathIsNull_Throw() => Ensure.WhenIsNullOrWhiteSpace(path =>
    {
        //Arrange
        var options = Fixture.Create<FileFetchOptions>();

        //Act
        var action = () => Instance.Fetch(path, options);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(path));
    });

    [TestMethod]
    public void FetchWithOptions_WhenDirectoryDoesNotExist_ReturnEmpty()
    {
        //Arrange
        var path = Fixture.Create<string>();
        var options = Fixture.Create<FileFetchOptions>();

        GetMock<IDirectory>().Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
        GetMock<IDirectory>().Setup(x => x.Exists(path)).Returns(false);

        //Act
        var result = Instance.Fetch(path, options);

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void FetchWithOptions_WhenDirectoryExistsAndFileExtensionsEmpty_ReturnAllFiles()
    {
        //Arrange
        var path = Fixture.Create<string>();
        var options = Fixture.Build<FileFetchOptions>().With(x => x.FileExtensions, ImmutableList<string>.Empty).Create();

        GetMock<IDirectory>().Setup(x => x.Exists(path)).Returns(true);

        var files = Fixture.CreateMany<string>().ToList();
        GetMock<IDirectory>().Setup(x => x.EnumerateFiles(path, "*.*", options.SearchKind)).Returns(files);

        //Act
        var result = Instance.Fetch(path, options);

        //Assert
        result.Should().BeEquivalentTo(files.Select(x => new Uri(x, options.UriKind)));
    }

    [TestMethod]
    public void FetchWithOptions_WhenDirectoryExistsAndFileExtensionsNotEmpty_ReturnOnlyFilesWithExtensions()
    {
        //Arrange
        var path = Fixture.Create<string>();
        var options = Fixture.Create<FileFetchOptions>();

        GetMock<IDirectory>().Setup(x => x.Exists(path)).Returns(true);

        var files = Fixture.CreateMany<string>().ToList();
        GetMock<IDirectory>().Setup(x => x.EnumerateFiles(path, "*.*", options.SearchKind)).Returns(files);

        var filesWithExtension = Fixture.CreateMany<string>().ToList();
        foreach (var file in filesWithExtension)
            GetMock<IPath>().Setup(x => x.GetExtensionWithoutDot(file)).Returns(options.FileExtensions.GetRandom());

        files.AddRange(filesWithExtension);

        //Act
        var result = Instance.Fetch(path, options);

        //Assert
        result.Should().BeEquivalentTo(filesWithExtension.Select(x => new Uri(x, options.UriKind)));
    }
}