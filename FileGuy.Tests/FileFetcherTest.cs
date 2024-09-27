namespace FileGuy.Tests;

[TestClass]
public class FileFetcherTest : Tester<FileFetcher>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        GetMock<IOptions<FileFetchOptions>>().Setup(x => x.Value).Returns(new FileFetchOptions());
        Dummy.Exclude(UriKind.RelativeOrAbsolute);
    }

    [TestMethod]
    public void FetchWithFileExtensions_WhenPathIsNull_Throw() => Ensure.WhenIsNullOrWhiteSpace(path =>
    {
        //Arrange
        var fileExtensions = Dummy.CreateMany<string>().ToArray();

        //Act
        var action = () => Instance.Fetch(path, fileExtensions);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(path));
    });

    [TestMethod]
    public void FetchWithFileExtensions_WhenDirectoryDoesNotExist_ReturnEmpty()
    {
        //Arrange
        var path = Dummy.Create<string>();
        var fileExtensions = Dummy.CreateMany<string>().ToArray();

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
        var defaultOptions = Dummy.Create<FileFetchOptions>();
        GetMock<IOptions<FileFetchOptions>>().Setup(x => x.Value).Returns(defaultOptions);

        var path = Dummy.Create<string>();
        var fileExtensions = Array.Empty<string>();

        GetMock<IDirectory>().Setup(x => x.Exists(path)).Returns(true);

        var files = Dummy.Path.CreateMany().ToList();
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
        var defaultOptions = Dummy.Create<FileFetchOptions>();
        GetMock<IOptions<FileFetchOptions>>().Setup(x => x.Value).Returns(defaultOptions);

        var path = Dummy.Create<string>();
        var fileExtensions = Dummy.CreateMany<string>().ToArray();

        GetMock<IDirectory>().Setup(x => x.Exists(path)).Returns(true);

        var files = Dummy.Path.CreateMany().ToList();
        GetMock<IDirectory>().Setup(x => x.EnumerateFiles(path, "*.*", defaultOptions.SearchKind)).Returns(files);

        var filesWithExtension = Dummy.Path.WithFileName.WithExtension.OneOf(fileExtensions).CreateMany().ToList();
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
        var options = Dummy.Create<FileFetchOptions>();

        //Act
        var action = () => Instance.Fetch(path, options);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(path));
    });

    [TestMethod]
    public void FetchWithOptions_WhenDirectoryDoesNotExist_ReturnEmpty()
    {
        //Arrange
        var path = Dummy.Create<string>();
        var options = Dummy.Create<FileFetchOptions>();

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
        var path = Dummy.Create<string>();
        var options = Dummy.Build<FileFetchOptions>().With(x => x.FileExtensions, ImmutableList<string>.Empty).Create();

        GetMock<IDirectory>().Setup(x => x.Exists(path)).Returns(true);

        var files = Dummy.Path.CreateMany().ToList();
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
        var path = Dummy.Create<string>();
        var options = Dummy.Create<FileFetchOptions>();

        GetMock<IDirectory>().Setup(x => x.Exists(path)).Returns(true);

        var files = Dummy.Path.CreateMany().ToList();
        GetMock<IDirectory>().Setup(x => x.EnumerateFiles(path, "*.*", options.SearchKind)).Returns(files);

        var filesWithExtension = Dummy.Path.WithFileName.WithExtension.OneOf(options.FileExtensions.ToArray()).CreateMany().ToList();
        foreach (var file in filesWithExtension)
            GetMock<IPath>().Setup(x => x.GetExtensionWithoutDot(file)).Returns(options.FileExtensions.GetRandom());

        files.AddRange(filesWithExtension);

        //Act
        var result = Instance.Fetch(path, options);

        //Assert
        result.Should().BeEquivalentTo(filesWithExtension.Select(x => new Uri(x, options.UriKind)));
    }
}