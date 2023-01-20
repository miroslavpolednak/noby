using CIS.Infrastructure.gRPC;

namespace DomainServices.DocumentOnSAService.Api.Extensions;

internal static class GrpcExtensions
{
    public static void AddDocumentOnSAServiceGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddCisGrpcInfrastructure(typeof(Program));
        builder.Services.AddGrpc(options =>
        {
            options.Interceptors.Add<GenericServerExceptionInterceptor>();
        });
        builder.Services.AddGrpcReflection();
    }
}
