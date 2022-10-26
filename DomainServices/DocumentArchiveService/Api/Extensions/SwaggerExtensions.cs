using System.Reflection;
using Microsoft.OpenApi.Models;

namespace DomainServices.DocumentArchiveService.Api;

internal static class SwaggerExtensions
{
    static string xmlFileName(Type type) => type.GetTypeInfo().Module.Name.Replace(".dll", ".xml").Replace(".exe", ".xml");
    
    public static WebApplicationBuilder AddDocumentArchiveSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        // konfigurace pro generátor JSON souboru
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "DocumentArchive Service API", Version = "v1" });

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

            // generate the XML docs that'll drive the swagger docs
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName(typeof(Program))));
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DomainServices.DocumentArchiveService.Contracts.xml"));
        });
        
        return builder;
    }

    public static WebApplication UseDocumentArchiveSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "1.0");
        });
        return app;
    }
}
