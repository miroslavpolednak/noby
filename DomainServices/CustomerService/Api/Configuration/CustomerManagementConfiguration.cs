using System.Net.Http.Headers;
using System.Text;
using CIS.ExternalServicesHelpers.Configuration;
using DomainServices.CustomerService.Api.Clients;

namespace DomainServices.CustomerService.Api.Configuration;

public class CustomerManagementConfiguration : ExternalServiceBaseConfiguration
{
    public CMVersion Version { get; set; } = CMVersion.Unknown;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public AuthenticationHeaderValue HttpBasicAuth
    {
        get
        {
            var bytes = Encoding.ASCII.GetBytes($"{Username}:{Password}");

            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
        }
    }
}