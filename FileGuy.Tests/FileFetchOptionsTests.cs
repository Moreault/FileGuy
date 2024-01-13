namespace FileGuy.Tests;

[TestClass]
public sealed class FileFetchOptionsTests : RecordTester<FileFetchOptions>
{
    [TestMethod]
    public void FileExtensions_WhenValueIsNull_SetEmpty()
    {
        //Arrange

        //Act
        var result = new FileFetchOptions { FileExtensions = null! };

        //Assert
        result.FileExtensions.Should().BeEmpty();
    }

    [TestMethod]
    [DataRow(SearchOption.TopDirectoryOnly - 1)]
    [DataRow(SearchOption.AllDirectories + 1)]
    public void SearchKind_WhenValueIsUndefined_Throw(SearchOption searchKind)
    {
        //Arrange

        //Act
        var action = () => new FileFetchOptions { SearchKind = searchKind };

        //Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [DataRow(UriKind.RelativeOrAbsolute - 1)]
    [DataRow(UriKind.Relative + 1)]
    public void UriKind_WhenValueIsUndefined_Throw(UriKind uriKind)
    {
        //Arrange

        //Act
        var action = () => new FileFetchOptions { UriKind = uriKind };

        //Assert
        action.Should().Throw<ArgumentException>();
    }
}