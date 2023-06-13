using Microsoft.OpenApi.Models;

namespace DomainServices.UserService.Api;

internal static class SwaggerExtensions
{
    public static IServiceCollection AddUserServiceSwagger(this IServiceCollection services)
    {
        services.AddGrpcSwagger();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service API", Version = "v1" });

            // generate the XML docs that'll drive the swagger docs
            var path1 = Path.Combine(AppContext.BaseDirectory, "DomainServices.UserService.Contracts.xml");
            var path2 = Path.Combine(AppContext.BaseDirectory, "CIS.Infrastructure.gRPC.CisTypes.xml");

            c.IncludeXmlComments(path1);
            c.IncludeXmlComments(path2);
            c.IncludeGrpcXmlComments(path1, includeControllerXmlComments: true);
            c.IncludeGrpcXmlComments(path2, includeControllerXmlComments: true);

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

    public static WebApplication UseUserServiceSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
        });
        return app;
    }
}

