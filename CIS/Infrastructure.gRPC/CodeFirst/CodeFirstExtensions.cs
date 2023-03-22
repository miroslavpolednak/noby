using Microsoft.AspNetCore.Builder;

namespace CIS.Infrastructure.gRPC;

public static class CodeFirstExtensions
{
    public static GrpcServiceEndpointConventionBuilder MapCodeFirstGrpcHealthChecks(this WebApplication app)
    {
        return app.MapGrpcService<CodeFirst.GrpcCodeFirstHealthCheck>();
    }
}
