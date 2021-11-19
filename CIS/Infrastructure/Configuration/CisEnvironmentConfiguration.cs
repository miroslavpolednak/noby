namespace CIS.Infrastructure.Configuration
{
    internal class CisEnvironmentConfiguration : CIS.Core.Configuration.ICisEnvironmentConfiguration
    {
        public string? DefaultApplicationKey { get; set; } = null;

        public string? EnvironmentName { get; set; } = null;

        public string? ServiceDiscoveryUrl { get; set; } = null;

        public string? InternalServicesLogin { get; set; } = null;

        public string? InternalServicePassword { get; set; } = null;
    }
}
