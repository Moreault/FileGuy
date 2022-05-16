using ToolBX.FileGuy;

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
            var path = Fixture.Create<string>();
            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(false);

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

            var newFilename = @"c:\somedirectory\somefile (1).exe";
            GetMock<IFile>().Setup(x => x.Exists(newFilename)).Returns(false);

            //Act
            var result = Instance.Generate(path);

            //Assert
            result.Should().BeEquivalentTo(newFilename);
        }

        [TestMethod]
        public void WhenNewFileNameAlreadyExists_ReturnUniqueName()
        {
            //Arrange
            var path = "c:/somedirectory/somefile.exe";
            GetMock<IFile>().Setup(x => x.Exists(path)).Returns(true);

            var newFilename1 = @"c:\somedirectory\somefile (1).exe";
            GetMock<IFile>().Setup(x => x.Exists(newFilename1)).Returns(true);

            var newFilename2 = @"c:\somedirectory\somefile (2).exe";
            GetMock<IFile>().Setup(x => x.Exists(newFilename2)).Returns(false);

            //Act
            var result = Instance.Generate(path);

            //Assert
            result.Should().BeEquivalentTo(newFilename2);
        }
    }
}