using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.Notification.Api
{
    public static class MailingStartupExtension
    {
        public static IServiceCollection AddNotificationMailing(this IServiceCollection services, IConfiguration configuration)
        {
            /*var c = new BlobConfiguration();
            configuration.GetSection("Blob").Bind(c);

            // pridat konfiguraci do DI
            services.AddSingleton(c);

            // repo
            services.AddTransient<BlobRepository>();

            // pridat provider podle nastaveni configu
            switch (c.Provider)
            {
                case BlobConfiguration.Providers.LocalFilesystem:
                    services.AddTransient<IBlobStorageProvider, LocalFilesystemProvider>();
                    break;
            }*/

            return services;
        }
    }
}
