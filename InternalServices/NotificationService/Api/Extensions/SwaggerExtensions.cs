using Microsoft.OpenApi.Models;

namespace CIS.InternalServices.NotificationService.Api.Extensions;

internal static class SwaggerExtensions
{
    public static WebApplicationBuilder AddCustomSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "Notification Service API", Version = "v1" });
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
                    new string[] { }
                }
            });
            
            x.EnableAnnotations();
            x.DescribeAllParametersInCamelCase();
            x.UseInlineDefinitionsForEnums();

            x.CustomSchemaIds(type => type.ToString());

            x.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
        });

        return builder;
    }

    public static WebApplication UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "1.0"); });
        return app;
    }
}