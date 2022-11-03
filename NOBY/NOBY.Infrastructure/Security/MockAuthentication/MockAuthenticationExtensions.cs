using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace NOBY.Infrastructure.Security;

public static class MockAuthenticationExtensions
{
    public static AuthenticationBuilder AddFomsMockAuthentication(this IServiceCollection services)
    {
        return services
            .AddAuthentication(AuthenticationConstants.MockAuthScheme)
            .AddScheme<MockAuthenticationSchemeOptions, MockAuthenticationHandler>(AuthenticationConstants.MockAuthScheme, options => { });
    }
}
