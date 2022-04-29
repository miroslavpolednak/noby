using ProtoBuf.Grpc.Server;
using System.IO.Compression;
using System.Reflection;

namespace DomainServices.CodebookService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddCodebookService(this WebApplicationBuilder builder)
    {
        // add grpc
        builder.Services.AddCodeFirstGrpc(config => {
            config.ResponseCompressionLevel = CompressionLevel.Optimal;
            config.Interceptors.Add<CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor>();
        });

        return builder;
    }

    public static void AddCodebookServiceEndpointsStartup(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (Type type in assembly
              .GetTypes()
              .Where(mytype => mytype.GetInterfaces().Contains(typeof(Endpoints.ICodebookServiceEndpointStartup))))
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                ((Endpoints.ICodebookServiceEndpointStartup)assembly.CreateInstance(type.FullName ?? "")).Register(builder);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
        }
    }
}
