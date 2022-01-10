using CIS.Infrastructure.WebApi;
using System.Reflection;

namespace CIS.Infrastructure.StartupExtensions;

public static class WebApiExtensions
{
    public static IApplicationBuilder AddEndpointsModules(this IApplicationBuilder builder, params Assembly[] assemblies)
    {
        builder.UseRouting();
        builder.UseEndpoints(endpointRouteBuilder =>
        {
            foreach (var assembly in assemblies)
            {
                foreach (Type type in assembly
                  .GetTypes()
                  .Where(mytype => mytype.GetInterfaces().Contains(typeof(IApiEndpointModule))))
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    ((IApiEndpointModule)assembly.CreateInstance(type.FullName ?? "")).Register(endpointRouteBuilder);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                }
            }
        });

        return builder;
    }
}
