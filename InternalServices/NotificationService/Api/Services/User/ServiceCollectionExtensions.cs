using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;

namespace CIS.InternalServices.NotificationService.Api.Services.User;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddUserAdapter(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IUserAdapterService, UserAdapterService>();

        return builder;
    }
}