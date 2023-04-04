namespace CIS.Infrastructure.Configuration
{
    internal class CisEnvironmentConfiguration : CIS.Core.Configuration.ICisEnvironmentConfiguration
    {
        public bool DisableServiceDiscovery { get; set; }

        public string DefaultApplicationKey { get; set; }

        public string EnvironmentName { get; set; }

        public string ServiceDiscoveryUrl { get; set; }

        public string InternalServicesLogin { get; set; }

        public string InternalServicePassword { get; set; }
    }
}
