using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CIS.Infrastructure.Security;

internal sealed class CisServiceAuthenticationHandler 
    : AuthenticationHandler<CisServiceAuthenticationOptions>
{
    // cache pro ukladani paru login - heslo
    private static readonly ConcurrentDictionary<string, string> _loginPasswordCache = new();

    private readonly IAuthHeaderParser _headerParser;
    private readonly ILoginValidator _adValidator;
    private readonly ILogger<CisServiceAuthenticationHandler> _logger;

    public CisServiceAuthenticationHandler(
        IOptionsMonitor<CisServiceAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IAuthHeaderParser headerParser,
        ILoginValidator adValidator)
    : base(options, logger, encoder, clock)
    {
        _adValidator = adValidator;
        _headerParser = headerParser;
        _logger = logger.CreateLogger<CisServiceAuthenticationHandler>();
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // http header obsahuje Authorization key
        if (Context.Request.Headers.ContainsKey("Authorization"))
            return await HandleAuthenticateInternalAsync(Context.Request.Headers["Authorization"]!);
        else
        {
            _logger.AuthHeaderNotFound();
            return AuthenticateResult.NoResult();
        }
    }

    internal async Task<AuthenticateResult> HandleAuthenticateInternalAsync(string authorizationHeader)
    {
        var loginResult = _headerParser.Parse(authorizationHeader);

        if (!loginResult.Success) // nepodarilo se parsovat login a heslo
            return AuthenticateResult.Fail(loginResult.ErrorMessage);
        else if (!await authenticateUser(loginResult.Login, loginResult.Password)) // nepodarila se autentizace proti AD
            return AuthenticateResult.Fail("Login or password incorrect");
        else
        {
            // vytvorit identity
            var claimsIdentity = new ServiceUser.CisServiceIdentity(loginResult.Login);

            // vratit autentizacni ticket
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), InternalServicesAuthentication.DefaultSchemeName);
            return AuthenticateResult.Success(ticket);
        }
    }

    internal static void ClearLoginsCache()
    {
        _loginPasswordCache.Clear();
    }

    /// <summary>
    /// Autentizace uzivatele proti AD
    /// </summary>
    /// <returns>True pokud se podarilo overit username/password</returns>
    private async Task<bool> authenticateUser(string login, string password)
    {
        if (_loginPasswordCache.TryGetValue(login, out string? storedPassword)) // nejdriv hledat v cache
        {
            return storedPassword == password;
        }
        else if (await _adValidator.Validate(login, password)) // validace proti AD
        {
            _loginPasswordCache.TryAdd(login, password); // pridat do cache
            return true;
        }
        else
        {
            _logger.AdConnectionFailed(login, null);
            return false;
        }
    }
}
