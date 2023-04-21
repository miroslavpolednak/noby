using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NOBY.Infrastructure.Services.FlowSwitches;

namespace NOBY.Infrastructure.Services;

public static class StartupExtensions
{
    public static IServiceCollection AddFlowSwitches(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IFlowSwitchesCache>(new FlowSwitchesCache(connectionString));

        services.AddTransient<IFlowSwitchesService, FlowSwitchesService>();

        return services;
    }
}
