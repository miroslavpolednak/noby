using System.Net.Http.Headers;
using System.Text;
using CIS.ExternalServicesHelpers.Configuration;

namespace DomainServices.CustomerService.Api.Configuration;

public class CustomerManagementConfiguration : ExternalServiceBaseConfiguration
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public Clients.CustomerManagement.Version CustomerManagementVersion { get; set; }

    public Clients.CustomerProfile.Version CustomerProfileVersion { get; set; }

    public Clients.IdentifiedSubjectBr.Version IdentifiedSubjectVersion { get; set; }

    public AuthenticationHeaderValue HttpBasicAuth
    {
        get
        {
            var bytes = Encoding.ASCII.GetBytes($"{Username}:{Password}");

            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
        }
    }
}