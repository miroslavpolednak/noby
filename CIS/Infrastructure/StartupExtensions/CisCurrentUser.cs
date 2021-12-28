﻿using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.StartupExtensions
{
    public static class CisCurrentUser
    {
        /// <summary>
        /// Registrace sluzby pro ziskani instance uzivatele
        /// </summary>
        public static IServiceCollection AddCisCurrentUser(this IServiceCollection services)
        {
            services.TryAddTransient<CIS.Core.Security.ICurrentUserAccessor, Security.CisCurrentUserAccessor>();

            return services;
        }
    }
}
