using CIS.Infrastructure.Configuration.Providers;

namespace CIS.Infrastructure.Configuration;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables (target=Machine)
    /// with a specified prefix.
    /// </summary>
    /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="prefix">The prefix that environment variable names must start with. The prefix will be removed from the environment variable names.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddCisEnvironmentVariables(
        this IConfigurationBuilder configurationBuilder,
        string? prefix)
    {
        configurationBuilder.Add(new CisEnvironmentVariablesConfigurationSource { Prefix = prefix });
        return configurationBuilder;
    }
}
