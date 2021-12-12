using CIS.Infrastructure.StartupExtensions;
using ExternalServices.MpHome;
using FluentValidation;
using System.IO.Compression;

namespace DomainServices.CustomerService.Api
{
    internal static class StartupExtensions
    {
        /// <summary>
        /// Kontrola zda je vse v konfiguracnim souboru korektne
        /// </summary>
        public static void CheckAppConfiguration(this AppConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("AppConfiguration");
        }

        public static IServiceCollection AddCutomerService(this IServiceCollection services, AppConfiguration appConfiguration, IConfiguration configuration)
        {
            services
                .AddMediatR(typeof(Program).Assembly)
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

            // add repos helpers
            services.AddDapper<Repositories.KonsDbRepository>(configuration.GetConnectionString("KonsDb"));

            // MpHome
            services.AddExternalServiceEas(appConfiguration.MpHome);

            // EAS svc
            switch (appConfiguration.EAS.Implementation)
            {
                case CIS.Core.ServiceImplementationTypes.Mock:
                    services.AddScoped<ExternalServices.EAS.IEasClient, ExternalServices.EAS.MockEasClient>();
                    break;
                case CIS.Core.ServiceImplementationTypes.Real:
                    services.AddScoped<ExternalServices.EAS.IEasClient, ExternalServices.EAS.RealEasClient>();
                    break;
            }

            services.AddCisCurrentUser();

            // add validators
            services.Scan(selector => selector
                    .FromAssembliesOf(typeof(IApiAssembly))
                    .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            return services;
        }
    }
}
