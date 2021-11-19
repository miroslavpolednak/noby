using CIS.Core.Types;
using Microsoft.Extensions.Configuration;

namespace CIS.Infrastructure.Caching.Tests;

public static class Constants
{
    static Constants()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        IConfiguration config = builder.Build();

        RedisConnectionString = config.GetValue<string>("RedisConnectionString");
        Environment = new ApplicationEnvironmentName(config.GetValue<string>("ApplicationEnvironmentName"));
        ApplicationKey = new ApplicationKey(config.GetValue<string>("ApplicationKey"));
    }

    internal static string RedisConnectionString { get; private set; }

    internal static ApplicationEnvironmentName Environment { get; private set; }

    internal static ApplicationKey ApplicationKey { get; private set; }
}
