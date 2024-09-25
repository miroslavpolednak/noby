using SharedAudit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;
using System.Security.Claims;

namespace NOBY.Api.Endpoints.Users.SignIn;

internal sealed class SignInHandler(
    ILogger<SignInHandler> _logger,
    DomainServices.UserService.Clients.v1.IUserServiceClient _userService,
    IHttpContextAccessor _httpContext,
    Infrastructure.Configuration.AppConfiguration _configuration,
    IAuditLogger _auditLogger)
        : IRequestHandler<UsersSignInRequest>
{
    public async Task Handle(UsersSignInRequest request, CancellationToken cancellationToken)
    {
        _auditLogger.Log(AuditEventTypes.Noby001, "Pokus o přihlášení uživatele");

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
            throw new CisAuthorizationException("APPLICATION_BasicAccess check failed");
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

        logAuditEvent(request);
    }

    private void logAuditEvent(UsersSignInRequest request)
    {
        if (string.IsNullOrEmpty(_appVersion))
        {
            _appVersion = Assembly.GetEntryAssembly()!.GetName().Version!.ToString();
        }

        _auditLogger.Log(
            AuditEventTypes.Noby002,
            $"Uživatel {request.IdentityScheme}={request.IdentityId} se přihlásil do aplikace.",
            bodyAfter: new Dictionary<string, string>() {
                { "login", $"{request.IdentityScheme}={request.IdentityId}" },
                { "type", AuthenticationConstants.SimpleLoginAuthScheme },
                { "app_version", _appVersion }
            });
    }

    private static string? _appVersion;
}
