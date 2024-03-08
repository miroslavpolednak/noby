using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.FeatureManagement;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisCoreFeatures
{
    /// <summary>
    /// Adds CIS core features to the WebApplicationBuilder.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    /// <param name="addDefaultTimeProvider">Indicates whether to add the default time provider.</param>
    /// <param name="addFeatureManagement">Indicates whether to add feature management.</param>
    /// <returns>The modified WebApplicationBuilder instance.</returns>
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
