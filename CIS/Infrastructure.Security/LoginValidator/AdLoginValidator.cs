using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace CIS.Infrastructure.Security;

/// <summary>
/// Validace login/pwd proti AD
/// </summary>
internal sealed class AdLoginValidator 
    : ILoginValidator
{
    private readonly ILogger<ILoginValidator> _logger;
    private IOptionsMonitor<CisServiceAuthenticationOptions> _options;

    public AdLoginValidator(ILogger<ILoginValidator> logger, IOptionsMonitor<CisServiceAuthenticationOptions> options)
    {
        _options = options;
        _logger = logger;
    }

    public async Task<bool> Validate(string login, string password)
    {
        var opt = _options.Get(InternalServicesAuthentication.DefaultSchemeName);

        try
        {
            using (var cn = new LdapConnection())
            {
                cn.UserDefinedServerCertValidationDelegate += new RemoteCertificateValidationCallback((sender, certificate, chain, errors) => true);
                cn.SecureSocketLayer = opt.IsSsl;
                await cn.ConnectAsync(opt.AdHost, opt.AdPort);
                await cn.BindAsync($"{opt.Domain}\\{login}", password);

                return cn.Bound;
            }
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
            return false;
        }
    }
}
