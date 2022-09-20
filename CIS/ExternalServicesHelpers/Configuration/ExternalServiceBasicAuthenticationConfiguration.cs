using System.Net.Http.Headers;
using System.Text;

namespace CIS.ExternalServicesHelpers.Configuration;

public abstract class ExternalServiceBasicAuthenticationConfiguration
    : ExternalServiceBaseConfiguration, IExternalServiceBasicAuthenticationConfiguration
{
    public string? Username { get; set; }

    public string? Password { get; set; }

    public AuthenticationHeaderValue HttpBasicAuthenticationHeader
    {
        get
        {
            if (_httpBasicAuthenticationHeader is null)
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                    throw new ArgumentNullException("Username or Password for Basic Authentication has not been set");

                var bytes = Encoding.ASCII.GetBytes($"{Username}:{Password}");
                _httpBasicAuthenticationHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
            }
            
            return _httpBasicAuthenticationHeader;
        }
    }
    private AuthenticationHeaderValue? _httpBasicAuthenticationHeader;
}
