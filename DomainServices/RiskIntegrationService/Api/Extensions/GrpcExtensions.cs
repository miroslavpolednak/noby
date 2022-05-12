using ProtoBuf.Grpc.Server;
using System.IO.Compression;

namespace DomainServices.RiskIntegrationService.Api;

internal static class GrpcExtensions
{
    public static void AddRipGrpc(this WebApplicationBuilder builder)
    {
        // add grpc
        builder.Services.AddCodeFirstGrpc(config => {
            config.ResponseCompressionLevel = CompressionLevel.Optimal;
            config.Interceptors.Add<CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor>();
        });

        // add grpc reflection
        builder.Services.AddCodeFirstGrpcReflection();
    }
}
