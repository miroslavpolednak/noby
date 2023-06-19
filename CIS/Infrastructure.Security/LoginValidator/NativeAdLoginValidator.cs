using Microsoft.Extensions.Options;
using System.DirectoryServices.Protocols;

namespace CIS.Infrastructure.Security.LoginValidator;

internal sealed class NativeAdLoginValidator
    : ILoginValidator
{
    private readonly ILogger<ILoginValidator> _logger;
    private IOptionsMonitor<CisServiceAuthenticationOptions> _options;

    public NativeAdLoginValidator(ILogger<ILoginValidator> logger, IOptionsMonitor<CisServiceAuthenticationOptions> options)
    {
        _options = options;
        _logger = logger;
    }

    public Task<bool> Validate(string login, string password)
    {
        var opt = _options.Get(InternalServicesAuthentication.DefaultSchemeName);

        try
        {
            using (var ldapConnection = new LdapConnection($"{opt.AdHost}:{opt.AdPort}"))
            {
                ldapConnection.AuthType = AuthType.Negotiate;
                ldapConnection.SessionOptions.ProtocolVersion = 3;
                ldapConnection.SessionOptions.SecureSocketLayer = opt.IsSsl;
                ldapConnection.SessionOptions.VerifyServerCertificate += (sender, certificate) => true;

                ldapConnection.Bind(new System.Net.NetworkCredential(login, password, opt.Domain));
            }

            return Task.FromResult(true);
        }
        catch (Exception err)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "AdHost", opt.AdHost ?? "" },
                { "AdPort", opt.AdPort },
                { "Domain", opt.Domain ?? "" },
                { "Password", password }
            }))
            {
                _logger.AdConnectionFailed(login, err);
            }
            return Task.FromResult(false);
        }
    }
}
