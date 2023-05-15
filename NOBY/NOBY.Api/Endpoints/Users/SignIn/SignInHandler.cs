using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace NOBY.Api.Endpoints.Users.SignIn;

internal sealed class SignInHandler 
    : IRequestHandler<SignInRequest>
{
    public async Task Handle(SignInRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.IdentityId))
        {
            request.IdentityId = request.Login;
        }
        if (_configuration.Security!.AuthenticationScheme != NOBY.Infrastructure.Security.AuthenticationConstants.SimpleLoginAuthScheme)
        {
            throw new NobyValidationException($"SignIn endpoint call is not enabled for scheme {_configuration.Security!.AuthenticationScheme}");
        }

        if (string.IsNullOrEmpty(request.IdentityScheme))
        {
            request.IdentityScheme = "OsCis";
        }

        string login = $"{request.IdentityScheme}={request.IdentityId}";
        _logger.UserSigningInAs(login);

        var userInstance = await _userService.GetUser(login, cancellationToken);
        if (userInstance is null) throw new CisValidationException("Login not found");

        var claims = new List<Claim>
        {
            // natvrdo zadat login, protoze request.Login obsahuje CPM
            new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeIdent, login),
            new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeId, userInstance.UserId.ToString(System.Globalization.CultureInfo.InvariantCulture))
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContext.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }

    private readonly IHttpContextAccessor _httpContext;
    private readonly ILogger<SignInHandler> _logger;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;
    private readonly Infrastructure.Configuration.AppConfiguration _configuration;

    public SignInHandler(ILogger<SignInHandler> logger, DomainServices.UserService.Clients.IUserServiceClient userService, IHttpContextAccessor httpContext, Infrastructure.Configuration.AppConfiguration configuration)
    {
        _configuration = configuration;
        _httpContext = httpContext;
        _logger = logger;
        _userService = userService;
    }
}
