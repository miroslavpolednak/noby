using CIS.Infrastructure.Security.Configuration;
using Microsoft.Extensions.Options;
using System.DirectoryServices.Protocols;

namespace CIS.Infrastructure.Security.LoginValidator;

internal sealed class NativeAdLoginValidator
    : ILoginValidator
{
    private readonly ILogger<ILoginValidator> _logger;
    private CisServiceAuthenticationConfiguration _configuration;

    public NativeAdLoginValidator(ILogger<ILoginValidator> logger, IOptions<CisServiceAuthenticationConfiguration> configuration)
    {
        _configuration = configuration.Value;
        _logger = logger;
    }

    public Task<bool> Validate(string login, string password)
    {
        try
        {
            using (var ldapConnection = new LdapConnection($"{_configuration.AdHost}:{_configuration.AdPort}"))
            {
                ldapConnection.AuthType = AuthType.Negotiate;
                ldapConnection.SessionOptions.ProtocolVersion = 3;
                ldapConnection.SessionOptions.SecureSocketLayer = _configuration.IsSsl;
                ldapConnection.SessionOptions.VerifyServerCertificate += (sender, certificate) => true;

                ldapConnection.Bind(new System.Net.NetworkCredential(login, password, _configuration.Domain));
            }

            return Task.FromResult(true);
        }
        catch (Exception err)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "AdHost", _configuration.AdHost ?? "" },
                { "AdPort", _configuration.AdPort ?? 0 },
                { "Domain", _configuration.Domain ?? "" },
                { "Password", password }
            }))
            {
                _logger.AdConnectionFailed(login, err);
            }
            return Task.FromResult(false);
        }
    }
}
