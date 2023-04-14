using CIS.Core.Exceptions;

namespace DomainServices.CustomerService.Api.Configuration;

internal sealed class AppConfiguration
{
    public string CustomerManagementEventTopic { get; set; } = null!;

    public void Validate()
    {
        if (string.IsNullOrEmpty(CustomerManagementEventTopic))
        {
            throw new CisConfigurationException(0, $"{nameof(CustomerManagementEventTopic)} is empty.");
        }
    }
}
