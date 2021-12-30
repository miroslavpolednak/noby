using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.StartupExtensions
{
    public static class CisCurrentUser
    {
        /// <summary>
        /// Registrace sluzby pro ziskani instance uzivatele
        /// </summary>
        public static WebApplicationBuilder AddCisCurrentUser(this WebApplicationBuilder builder)
        {
            builder.Services.TryAddTransient<CIS.Core.Security.ICurrentUserAccessor, Security.CisCurrentUserAccessor>();

            return builder;
        }
    }
}
