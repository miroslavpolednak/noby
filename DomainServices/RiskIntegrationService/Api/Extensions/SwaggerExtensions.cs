using System.Reflection;
using Microsoft.OpenApi.Models;

namespace DomainServices.RiskIntegrationService.Api;

internal static class SwaggerExtensions
{
    static string xmlFileName(Type type) => type.GetTypeInfo().Module.Name.Replace(".dll", ".xml").Replace(".exe", ".xml");
    
    public static WebApplicationBuilder AddRipSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        // konfigurace pro generátor JSON souboru
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "Risk Integration Platform API", Version = "v1" });

            x.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "CIS service user login"
            });
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            Array.Empty<string>()
                    }
                });

            // zapojení rozšířených anotací nad controllery
            x.EnableAnnotations();
            //x.UseOneOfForPolymorphism();

            // všechny parametry budou camel case
            x.DescribeAllParametersInCamelCase();
            x.UseInlineDefinitionsForEnums();

            x.CustomSchemaIds(type => type.ToString());

            x.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });

            // generate the XML docs that'll drive the swagger docs
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName(typeof(Program))));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DomainServices.RiskIntegrationService.Contracts.xml"));
        });
        
        return builder;
    }

    public static WebApplication UseRipSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "1.0");
        });
        return app;
    }
}
