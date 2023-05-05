using CIS.Core.Configuration;

namespace Console_NotificationService
{
    internal class CisEnvironmentConfiguration : ICisEnvironmentConfiguration
    {
        public bool DisableServiceDiscovery { get; set; }

        public string? DefaultApplicationKey { get; set; }

        public string? EnvironmentName { get; set; }

        public string? ServiceDiscoveryUrl { get; set; }

        public string? InternalServicesLogin { get; set; }

        public string? InternalServicePassword { get; set; }
    }
}
