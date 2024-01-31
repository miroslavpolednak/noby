using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.FeatureManagement;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisCoreFeatures
{
    public static WebApplicationBuilder AddCisCoreFeatures(this WebApplicationBuilder builder, bool addDefaultTimeProvider = false, bool addFeatureManagement = false)
    {
        // datetime unification
        if (addDefaultTimeProvider)
        {
            builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);
        }

        builder.Services.AddOptions();

        builder.Services.Configure<JsonOptions>(x =>
        {
            x.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            x.JsonSerializerOptions.IncludeFields = false;
            x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        builder.Services.AddHttpContextAccessor();

        // feature flags
        if (addFeatureManagement)
        {
            builder.Services.AddFeatureManagement();
        }

        return builder;
    }
}
