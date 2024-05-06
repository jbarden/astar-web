namespace AStar.Utilities;

public class ObjectExtensionsShould
{
    [Fact]
    public void ConvertTheSpedifiedObjectToTheExpectedJson()
    {
        var testData = new {Id =1, Name ="Test"};

        testData.ToJson().Should().Be("{\"Id\":1,\"Name\":\"Test\"}");
    }
}
