using Microsoft.Extensions.Configuration;

namespace Xenia.WebApi.PerformanceTests;

public static class PerformanceTestSetup
{
    public static IConfigurationRoot GetConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Performance.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
}