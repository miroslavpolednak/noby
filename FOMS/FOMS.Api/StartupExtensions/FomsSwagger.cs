using Microsoft.OpenApi.Models;

namespace FOMS.Api.StartupExtensions;

internal static class FomsSwagger
{
    public static WebApplicationBuilder AddFomsSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        // konfigurace pro generátor JSON souboru
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "NOBY FRONTEND API", Version = "v1" });

            // všechny parametry budou camel case
            x.DescribeAllParametersInCamelCase();

            x.CustomSchemaIds(type => type.ToString());
        });

        return builder;
    }
}
