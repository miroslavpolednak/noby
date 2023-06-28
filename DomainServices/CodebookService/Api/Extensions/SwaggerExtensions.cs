using Microsoft.OpenApi.Models;

namespace DomainServices.CodebookService.Api;

internal static class SwaggerExtensions
{
    public static IServiceCollection AddCodebookServiceSwagger(this IServiceCollection services)
    {
        services.AddGrpcSwagger();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Codebook Service API", Version = "v1" });

            // generate the XML docs that'll drive the swagger docs
            var path = Path.Combine(AppContext.BaseDirectory, "DomainServices.CodebookService.Contracts.xml");
            c.IncludeXmlComments(path);
            c.IncludeGrpcXmlComments(path, includeControllerXmlComments: true);

            c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        });

        return services;
    }

    public static WebApplication UseCodebookServiceSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
        });
        return app;
    }
}
