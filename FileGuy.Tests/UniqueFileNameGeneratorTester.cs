namespace FileGuy.Tests;

[TestClass]
public class UniqueFileNameGeneratorTester
{
    //TODO Find a way to make it use unix separators even on windows so that these tests will run correctly no matter the OS (or use preprocessor?)
    [TestClass]
    public class Generate : Tester<UniqueFileNameGenerator>
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void WhenFilenameIsEmpty_Throw(string filename)
        {
            //Arrange

            //Act
            var action = () => Instance.Generate(filename);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenOriginalFilenameDoesNotExist_ReturnOriginalFilename()
        {
            //Arrange
            var path = Dummy.Create<string>();
            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(false);

            var directory = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetDirectoryName(path)).Returns(directory);

            var filename = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetFileNameWithoutExtension(path)).Returns(filename);

            var extension = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetExtension(path)).Returns(extension);

            //Act
            var result = Instance.Generate(path);

            //Assert
            result.Should().BeEquivalentTo(path);
        }

        [TestMethod]
        public void WhenOriginalFilenameExists_ReturnUniqueName()
        {
            //Arrange
            var path = "c:/somedirectory/somefile.exe";
            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var directory = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetDirectoryName(path)).Returns(directory);

            var filename = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetFileNameWithoutExtension(path)).Returns(filename);

            var extension = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetExtension(path)).Returns(extension);

            var combined = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.Combine(directory, $"{filename} (1).{extension}")).Returns(combined);
            
            GetMock<IFile>().Setup(x => x.Exists(combined)).Returns(false);

            //Act
            var result = Instance.Generate(path);

            //Assert
            result.Should().BeEquivalentTo(combined);
        }

        [TestMethod]
        public void WhenNewFileNameAlreadyExists_ReturnUniqueName()
        {
            //Arrange
            var path = "c:/somedirectory/somefile.exe";
            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var directory = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetDirectoryName(path)).Returns(directory);

            var filename = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetFileNameWithoutExtension(path)).Returns(filename);

            var extension = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.GetExtension(path)).Returns(extension);

            var combined1 = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.Combine(directory, $"{filename} (1).{extension}")).Returns(combined1);
            var combined2 = Dummy.Create<string>();
            GetMock<IPath>().Setup(x => x.Combine(directory, $"{filename} (2).{extension}")).Returns(combined2);

            GetMock<IFile>().Setup(x => x.Exists(combined1)).Returns(true);
            GetMock<IFile>().Setup(x => x.Exists(combined2)).Returns(false);

            //Act
            var result = Instance.Generate(path);

            //Assert
            result.Should().BeEquivalentTo(combined2);
        }
    }
}