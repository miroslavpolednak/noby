using CIS.Infrastructure.Audit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace NOBY.Api.Endpoints.Users.SignIn;

internal sealed class SignInHandler 
    : IRequestHandler<SignInRequest>
{
    public async Task Handle(SignInRequest request, CancellationToken cancellationToken)
    {
        _auditLogger.Log(CIS.Infrastructure.Audit.AuditEventTypes.Noby001, "Pokus o přihlášení uživatele");

        if (_configuration.Security!.AuthenticationScheme != AuthenticationConstants.SimpleLoginAuthScheme)
        {
            throw new NobyValidationException($"SignIn endpoint call is not enabled for scheme {_configuration.Security!.AuthenticationScheme}");
        }

        string login = $"{request.IdentityScheme}={request.IdentityId}";
        _logger.UserSigningInAs(login);

        var userInstance = await _userService.GetUser(login, cancellationToken);
        if (userInstance is null) throw new CisValidationException("Login not found");

        var permissions = await _userService.GetUserPermissions(userInstance.UserId, cancellationToken);
        // kontrola, zda ma uzivatel pravo na aplikaci jako takovou
        if (!permissions.Contains((int)UserPermissions.APPLICATION_BasicAccess))
        {
            throw new CisAuthorizationException();
        }

        var claims = new List<Claim>
        {
            // natvrdo zadat login, protoze request.Login obsahuje CPM
            new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeIdent, login),
            new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeId, userInstance.UserId.ToString(System.Globalization.CultureInfo.InvariantCulture))
        };
        // doplnit prava uzivatele do claims
        claims.AddRange(permissions.Select(t => new Claim(AuthenticationConstants.NobyPermissionClaimType, $"{t}")));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContext.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        _auditLogger.Log(
            CIS.Infrastructure.Audit.AuditEventTypes.Noby002,
            $"Uživatel {request.IdentityScheme}={request.IdentityId} se přihlásil do aplikace.",
            bodyAfter: new Dictionary<string, string>() { 
                { "login", $"{request.IdentityScheme}={request.IdentityId}" },
                { "type", AuthenticationConstants.SimpleLoginAuthScheme }
            });
    }

    private readonly IHttpContextAccessor _httpContext;
    private readonly ILogger<SignInHandler> _logger;
    private readonly IAuditLogger _auditLogger;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly Infrastructure.Configuration.AppConfiguration _configuration;

    public SignInHandler(
        ILogger<SignInHandler> logger, 
        DomainServices.UserService.Clients.IUserServiceClient userService, 
        IHttpContextAccessor httpContext, 
        Infrastructure.Configuration.AppConfiguration configuration,
        IAuditLogger auditLogger)
    {
        _auditLogger = auditLogger;
        _configuration = configuration;
        _httpContext = httpContext;
        _logger = logger;
        _userService = userService;
    }
}
