using CIS.Core;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisCoreFeatures
{
    public static IServiceCollection AddCisCoreFeatures(this IServiceCollection services)
    {
        // datetime unification
        services.AddSingleton<IDateTime, LocalDateTime>();

        services.AddOptions();

        services.Configure<JsonOptions>(x =>
        {
            x.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            x.JsonSerializerOptions.IncludeFields = false;
            x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        return services;
    }
}
