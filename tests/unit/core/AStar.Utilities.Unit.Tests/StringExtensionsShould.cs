using System.Text.Json;

namespace AStar.Utilities;

public class StringExtensionsShould
{
    [Fact]
    public void ReturnTrueFromIsNullWhenTheSpecifiedStringIsNull()
    {
        string? nullString = null;

        nullString.IsNull().Should().BeTrue();
    }

    [Fact]
    public void ReturnFalseFromIsNullWhenTheSpecifiedStringIsNull()
    {
        var nullString = "";

        nullString.IsNull().Should().BeFalse();
    }

    [Fact]
    public void ReturnTrueFromIsNotNullWhenTheSpecifiedStringIsNotNull()
    {
        var nullString = "";

        nullString.IsNotNull().Should().BeTrue();
    }

    [Fact]
    public void ReturnFalseFromIsNotNullWhenTheSpecifiedStringIsNull()
    {
        string? nullString = null;

        nullString.IsNotNull().Should().BeFalse();
    }

    [Fact]
    public void ReturnTrueFromIsNullOrWhiteSpaceWhenTheSpecifiedStringIsNotNull()
    {
        var nullString = "   ";

        nullString.IsNullOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public void ReturnFalseFromIsNullOrWhiteSpaceWhenTheSpecifiedStringIsNull()
    {
        var nullString = "does not matter";

        nullString.IsNullOrWhiteSpace().Should().BeFalse();
    }

    [Fact]
    public void ReturnTrueFromIsNotNullOrWhiteSpaceWhenTheSpecifiedStringIsNotNullOrWhiteSpace()
    {
        var nullString = "does not matter";

        nullString.IsNotNullOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public void ReturnFalseFromIsNotNullOrWhiteSpaceWhenTheSpecifiedStringIsWhiteSpace()
    {
        var nullString = " ";

        nullString.IsNotNullOrWhiteSpace().Should().BeFalse();
    }

    [Fact]
    public void ReturnTheExpectedObjectFromSuppliedJsonUsingDefaultSerializationOptons()
    {
        var json = "{\"Id\":1,\"Name\":\"Test\"}";

        json.FromJson<AnyClass>().Should().BeEquivalentTo(new AnyClass { Id = 1, Name = "Test" });
    }

    [Fact]
    public void ReturnTheExpectedObjectFromSuppliedJsonUsingDefaultSerializationOptonsUsingSuppliedSerializationOptions()
    {
        var json = "{\"id\":1,\"name\":\"Test\"}";

        json.FromJson<AnyClass>(new(JsonSerializerDefaults.Web)).Should().BeEquivalentTo(new AnyClass { Id = 1, Name = "Test" });
    }

    private class AnyClass
    {
        public int Id { get; set; }

        public string Name { get; set; } = "Not set";
    }
}
