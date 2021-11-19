using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System.Globalization;

namespace CIS.Infrastructure.StartupExtensions
{
    public static class CisWebRequestLocalization
    {
        private static List<CultureInfo> _cultures = new List<CultureInfo>
        {
            new CultureInfo("cs-CZ"),
            new CultureInfo("cs")
        };

        public static IApplicationBuilder UseCisWebRequestLocalization(this IApplicationBuilder services)
        {
            return services.UseRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("cs-CZ");

                options.SupportedCultures = _cultures;
                options.SupportedUICultures = _cultures;

                options.RequestCultureProviders.Clear();
            });
        }
    }
}