namespace AStar.Clean.V1.DomainModel;

/// <summary>
/// The tests here do not cover every property - just those that are not covered elsewhere.
/// Either the code or other tests achieve the desired 100% coverage when combined by these tests.
/// It is not always necessary to write specific tests for each property - even in the Domain Model - as
/// the code will fail to build / coverage will drop if refactorings affect this class.
/// That said, if the domain model class contains logic, you should test it directly - as we do here.
/// </summary>
public class FileInfoJbShould
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("file.txt")]
    [InlineData("file.doc")]
    [InlineData("file.docx")]
    public void ReturnFalseForIsImageWhenFilenameIsNotAnImageType(string filename)
    {
        var sut = new FileDetail { FileName = filename };

        _ = sut.IsImage.Should().BeFalse();
    }

    [Theory]
    [InlineData("file.jpg")]
    [InlineData("file.jpeg")]
    [InlineData("file.gif")]
    [InlineData("file.jif")]
    [InlineData("file.bmp")]
    [InlineData("file.JPG")]
    [InlineData("file.JPEG")]
    [InlineData("file.GIF")]
    [InlineData("file.JIF")]
    [InlineData("file.BMP")]
    public void ReturnTrueForIsImageWhenFilenameIsAnImageTypeIgnoringCase(string filename)
    {
        var sut = new FileDetail { FileName = filename };

        _ = sut.IsImage.Should().BeTrue();
    }

    //[Fact]
    //public void ReturnTheFullNameAsTheConcatenatedDirectoryAndFileNameProperties()
    //{
    //    var sut = new FileDetail { DirectoryName = @"c:\temp", FileName = "test.txt" };

    //    _ = sut.FullName.Should().Be(@"c:\temp\test.txt");
    //}

    //[Fact]
    //public void ReturnNullForDimensionsWhenNotSpecified()
    //{
    //    var sut = new FileDetail();

    //    _ = sut.Dimensions.Should().BeNull();
    //}

    //[Fact]
    //public void ReturnSpecifiedDimensionsForDimensionsWhenSpecified()
    //{
    //    var sut = new FileDetail { Dimensions = new() { Height = 1, Width = 2 } };

    //    _ = sut.Dimensions.Should().BeEquivalentTo(new Dimensions { Height = 1, Width = 2 });
    //}
}
