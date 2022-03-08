using System.Reflection;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace FOMS.Api.StartupExtensions;

internal static class FomsSwagger
{
    static string xmlFileName(Type type) => type.GetTypeInfo().Module.Name.Replace(".dll", ".xml").Replace(".exe", ".xml");
    
    public static WebApplicationBuilder AddFomsSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        // konfigurace pro generátor JSON souboru
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "NOBY FRONTEND API", Version = "v1" });

            // zapojení rozšířených anotací nad controllery
            x.EnableAnnotations();

            // všechny parametry budou camel case
            x.DescribeAllParametersInCamelCase();
            x.UseInlineDefinitionsForEnums();

            x.CustomSchemaIds(type => type.ToString());
            
            // generate the XML docs that'll drive the swagger docs
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName(typeof(Program))));
        });
        
        // Adds FluentValidationRules staff to Swagger.
        builder.Services.AddFluentValidationRulesToSwagger();

        return builder;
    }
}
