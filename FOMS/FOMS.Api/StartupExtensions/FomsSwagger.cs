using Microsoft.OpenApi.Models;

namespace FOMS.Api.StartupExtensions;

internal static class FomsSwagger
{
    public static IServiceCollection AddFomsSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        // konfigurace pro generátor JSON souboru
        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "FOMS API", Version = "v1" });

            // všechny parametry budou camel case
            x.DescribeAllParametersInCamelCase();

            // zapojení rozšířených anotací nad controllery
            x.EnableAnnotations();

            // v případě konfliktů dogeneruje unikátní ID operací
            x.ResolveConflictingActions(descriptions =>
            {
                var description = descriptions.First();

                string rnd = Guid.NewGuid().ToString().Substring(0, 3);
                description.ActionDescriptor.DisplayName = description.ActionDescriptor.DisplayName + "_" + rnd;

                return description;
            });
        });

        return services;
    }

    internal static IApplicationBuilder UseFomsSwagger(this IApplicationBuilder builder)
    {
        builder.UseSwagger();
        builder.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "FOMS API V1");
        });

        return builder;
    }
}
