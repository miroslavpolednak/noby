using CIS.Infrastructure.StartupExtensions;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddUserService(this WebApplicationBuilder builder)
    {
        // db repo
        builder.Services.AddDapper(builder.Configuration.GetConnectionString("xxvvss"));

        return builder;
    }
}
