﻿namespace AStar.Web.UI.Configuration;

public class ApplicationConfigurationShould
{
    [Fact]
    public void ContainTheExpectedDefaultPropertiesAndValues()
        => new ApplicationConfiguration().ToString().Should().Be("AStar.Web.UI.Configuration.ApplicationConfiguration");
}
