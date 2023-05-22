using CIS.Infrastructure.StartupExtensions;
using DomainServices.UserService.Api.Database;

namespace DomainServices.UserService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddUserService(this WebApplicationBuilder builder)
    {
        // dbcontext
        builder.AddEntityFramework<UserServiceDbContext>();

        return builder;
    }
}
