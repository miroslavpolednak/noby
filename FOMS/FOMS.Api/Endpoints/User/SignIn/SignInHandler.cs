using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FOMS.Api.Endpoints.User.Handlers;

internal sealed class SignInHandler 
    : AsyncRequestHandler<Dto.SignInRequest>
{
    protected override async Task Handle(Dto.SignInRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Try to sign in as '{login}'", request.Login);

        var userInstance = CIS.Core.Results.ServiceCallResult.ResolveToDefault<DomainServices.UserService.Contracts.User>(await _userService.GetUserByLogin(request.Login ?? ""));
        if (userInstance is null) throw new CIS.Core.Exceptions.CisValidationException("Login not found");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userInstance.FullName),
            new Claim(ClaimTypes.Sid, userInstance.Id.ToString()),
            new Claim(ClaimTypes.Spn, userInstance.CPM),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContext.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }

    private readonly IHttpContextAccessor _httpContext;
    private readonly ILogger<SignInHandler> _logger;
    private readonly DomainServices.UserService.Abstraction.IUserServiceAbstraction _userService;

    public SignInHandler(ILogger<SignInHandler> logger, DomainServices.UserService.Abstraction.IUserServiceAbstraction userService, IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
        _logger = logger;
        _userService = userService;
    }
}
