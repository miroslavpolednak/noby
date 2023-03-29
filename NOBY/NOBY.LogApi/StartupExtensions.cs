using Microsoft.OpenApi.Models;
using System.Reflection;

namespace NOBY.LogApi;

internal static class StartupExtensions
{
    static string xmlFileName(Type type) => type.GetTypeInfo().Module.Name.Replace(".dll", ".xml").Replace(".exe", ".xml");

    public static void AddLogApiSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "NOBY LOG API", Version = "v1" });

            x.EnableAnnotations();
            x.DescribeAllParametersInCamelCase();

            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName(typeof(Program))));
        });
    }
}
