using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FOMS.Infrastructure.Security;

/// <summary>
/// Fake authentication for local development - when CAAS is out of question.
/// </summary>
public sealed class MockAuthenticationHandler
    : AuthenticationHandler<MockAuthSchemeOptions>
{
    public MockAuthenticationHandler(
            IOptionsMonitor<MockAuthSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] 
        {
            new Claim(ClaimTypes.NameIdentifier, "990614w")
        };

        var claimsIdentity = new ClaimsIdentity(claims, AuthenticationConstants.MockAuthScheme);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), AuthenticationConstants.MockAuthScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
