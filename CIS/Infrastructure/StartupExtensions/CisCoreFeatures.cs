using CIS.Core;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CIS.Infrastructure.StartupExtensions;

public static class CisCoreFeatures
{
    public static WebApplicationBuilder AddCisCoreFeatures(this WebApplicationBuilder builder, bool useDefaultDateTimeService = true)
    {
        // datetime unification
        if (useDefaultDateTimeService)
            builder.Services.AddSingleton<IDateTime, LocalDateTime>();

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

        return builder;
    }
}
