using Microsoft.Extensions.DependencyInjection;
using CIS.Core;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace CIS.Infrastructure.StartupExtensions
{
    public static class CisCoreFeatures
    {
        public static IServiceCollection AddCisCoreFeatures(this IServiceCollection services)
        {
            // datetime unification
            services.AddSingleton<IDateTime, LocalDateTime>();

            services.AddOptions();

            services.Configure<JsonOptions>(x =>
            {
                x.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
                x.JsonSerializerOptions.IncludeFields = false;
                x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            return services;
        }
    }
}
