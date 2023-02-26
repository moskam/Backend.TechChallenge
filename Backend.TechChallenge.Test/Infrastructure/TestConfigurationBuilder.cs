using Microsoft.Extensions.Configuration;

namespace Backend.TechChallenge.Test.Infrastructure;

internal static class TestConfigurationBuilder
{
    public static IConfigurationRoot Build()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .Build();

        return config;
    }
}
