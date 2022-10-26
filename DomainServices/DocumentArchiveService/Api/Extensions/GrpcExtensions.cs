using ProtoBuf.Grpc.Server;
using System.IO.Compression;

namespace DomainServices.DocumentArchiveService.Api;

internal static class GrpcExtensions
{
    public static void AddDocumentArchiveGrpc(this WebApplicationBuilder builder)
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
