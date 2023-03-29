using CIS.Core.Configuration;

namespace Console_DocumentDataAggregator;

public class CisEnvironmentConfiguration : ICisEnvironmentConfiguration
{
    public string? DefaultApplicationKey => "console";

    public string? EnvironmentName => "DEV";

    public string? ServiceDiscoveryUrl => "https://localhost:5005";

    public string? InternalServicesLogin => "XX_NOBY_RMT_USR_TEST";

    public string? InternalServicePassword => "ppmlesnrTWYSDYGDR!98538535634544";
}