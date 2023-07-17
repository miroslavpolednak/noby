using DomainServices.UserService.Clients.Authorization;
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
            new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeId, "65466"),
            new Claim(CIS.Core.Security.SecurityConstants.ClaimTypeIdent, "KBUID=A09V61"),
            new Claim(AuthenticationConstants.NobyPermissionClaimType, $"{(int)UserPermissions.APPLICATION_BasicAccess}")
        };

        var claimsIdentity = new ClaimsIdentity(claims, AuthenticationConstants.MockAuthScheme);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), AuthenticationConstants.MockAuthScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
