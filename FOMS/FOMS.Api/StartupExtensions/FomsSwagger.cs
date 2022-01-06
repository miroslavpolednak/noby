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

            // zapojení rozšířených anotací nad controllery
            //x.EnableAnnotations();

            x.CustomSchemaIds(type => type.ToString());

            // v případě konfliktů dogeneruje unikátní ID operací
            /*x.ResolveConflictingActions(descriptions =>
            {
                var description = descriptions.First();

                string rnd = Guid.NewGuid().ToString().Substring(0, 3);
                description.ActionDescriptor.DisplayName = description.ActionDescriptor.DisplayName + "_" + rnd;

                return description;
            });*/
        });

        return builder;
    }
}
