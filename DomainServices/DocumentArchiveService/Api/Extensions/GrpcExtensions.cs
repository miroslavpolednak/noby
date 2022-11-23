using CIS.Infrastructure.gRPC;
using Microsoft.OpenApi.Models;

namespace DomainServices.DocumentArchiveService.Api;

internal static class GrpcExtensions
{
    public static void AddDocumentArchiveGrpc(this WebApplicationBuilder builder)
    {

        builder.Services.AddGrpc(options =>
        {
            options.Interceptors.Add<GenericServerExceptionInterceptor>();
        })
        .AddJsonTranscoding(opt =>
        {
            opt.JsonSettings.WriteEnumsAsIntegers = true;
        });

        builder.Services.AddGrpcReflection();
    }

    public static WebApplicationBuilder AddDocumentArchiveGrpcSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpcSwagger();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                            new OpenApiInfo { Title = "DocumentArchive Service API", Version = "v1" });

            // generate the XML docs that'll drive the swagger docs
            var path = Path.Combine(AppContext.BaseDirectory, "DomainServices.DocumentArchiveService.xml");
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
                            new string[] {}
                    }
                });
        });

        return builder;
    }

    public static WebApplication UseDocumentArchiveGrpcSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
        });
        return app;
    }
}
