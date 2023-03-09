using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace NOBY.Infrastructure.Security;

/// <summary>
/// Fake authentication for local development - when CAAS is out of question.
/// </summary>
public sealed class MockAuthenticationHandler
    : AuthenticationHandler<MockAuthenticationSchemeOptions>
{
    public MockAuthenticationHandler(
            IOptionsMonitor<MockAuthenticationSchemeOptions> options,
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
            new Claim(CIS.Core.Security.SecurityConstants.ClaimNameIdent, "KBUID=A09FK3")
        };

        var claimsIdentity = new ClaimsIdentity(claims, AuthenticationConstants.MockAuthScheme);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), AuthenticationConstants.MockAuthScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
