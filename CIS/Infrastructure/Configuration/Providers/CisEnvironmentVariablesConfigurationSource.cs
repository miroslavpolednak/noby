namespace CIS.Infrastructure.Configuration.Providers;

internal class CisEnvironmentVariablesConfigurationSource 
    : IConfigurationSource
{
    /// <summary>
    /// A prefix used to filter environment variables.
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Builds the <see cref="EnvironmentVariablesConfigurationProvider"/> for this source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
    /// <returns>A <see cref="EnvironmentVariablesConfigurationProvider"/></returns>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new CisEnvironmentVariablesConfigurationProvider(Prefix);
    }
}

