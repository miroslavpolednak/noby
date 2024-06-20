using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NOBY.Api.Endpoints.Users.SignOut;

internal sealed class SignOutHandler(
    IHttpContextAccessor _httpContext,
    Infrastructure.Configuration.AppConfiguration _configuration)
    : IRequestHandler<SignOutRequest>
{
    public async Task Handle(SignOutRequest request, CancellationToken cancellationToken)
    {
        if (_configuration.Security!.AuthenticationScheme != AuthenticationConstants.SimpleLoginAuthScheme)
        {
            throw new NobyValidationException($"SignIn endpoint call is not enabled for scheme {_configuration.Security!.AuthenticationScheme}");
        }

        await _httpContext.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
