using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace CIS.Infrastructure.Security;

/// <summary>
/// Validace login/pwd proti AD
/// </summary>
internal class AdLoginValidator : ILoginValidator
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
        try
        {
            var opt = _options.Get(InternalServicesAuthentication.DefaultSchemeName);

            using (var cn = new LdapConnection())
            {
                await cn.ConnectAsync(opt.AdHost, opt.AdPort);
                await cn.BindAsync($"{opt.DomainUsernamePrefix}{login}", password);

                return cn.Bound;
            }
        }
        catch (Exception err)
        {
            _logger.AdConnectionFailed(login, err);
            return false;
        }
    }
}
