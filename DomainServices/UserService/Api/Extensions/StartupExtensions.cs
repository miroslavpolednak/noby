using CIS.Infrastructure.Caching;
using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.ServiceDiscovery.Clients;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddUserService(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

        // db repo
        builder.Services.AddDapper(builder.Configuration.GetConnectionString("xxvvss"));

        return builder;
    }
}
