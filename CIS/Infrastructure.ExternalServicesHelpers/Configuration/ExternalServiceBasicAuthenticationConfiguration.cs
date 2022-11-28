namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

public class ExternalServiceBasicAuthenticationConfiguration<TClient>
    : ExternalServiceConfiguration<TClient>, IExternalServiceBasicAuthenticationConfiguration
    where TClient : class
{
    public string? Username { get; set; }

    public string? Password { get; set; }
}
