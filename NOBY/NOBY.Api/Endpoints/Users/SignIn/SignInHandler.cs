using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace NOBY.Api.Endpoints.Users.SignIn;

internal sealed class SignInHandler 
    : AsyncRequestHandler<SignInRequest>
{
    protected override async Task Handle(SignInRequest request, CancellationToken cancellationToken)
    {
        _logger.UserSigningInAs(request.Login);

        var userInstance = ServiceCallResult.ResolveToDefault<DomainServices.UserService.Contracts.User>(await _userService.GetUserByLogin(request.Login ?? "", cancellationToken));
        if (userInstance is null) throw new CisValidationException("Login not found");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userInstance.FullName),
            new Claim(ClaimTypes.NameIdentifier, userInstance.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)),
            new Claim(ClaimTypes.Spn, userInstance.CPM),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContext.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }

    private readonly IHttpContextAccessor _httpContext;
    private readonly ILogger<SignInHandler> _logger;
    private readonly DomainServices.UserService.Clients.IUserServiceClient _userService;

    public SignInHandler(ILogger<SignInHandler> logger, DomainServices.UserService.Clients.IUserServiceClient userService, IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
        _logger = logger;
        _userService = userService;
    }
}
