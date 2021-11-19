using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CIS.Infrastructure.StartupExtensions;
using ProtoBuf.Grpc.Server;
using System.IO.Compression;

namespace CIS.InternalServices.Notification.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // health checks
            services.AddCisHealthChecks(_configuration);

            services.AddMediatR(typeof(Startup).Assembly);

            services.AddCisCoreFeatures();

            // add repos helpers
            services.AddDapper(_configuration.GetConnectionString("default"));

            // add storage services
            //services.AddBlobStorage(_configuration);

            services.AddCodeFirstGrpc(config => {
                config.ResponseCompressionLevel = CompressionLevel.Optimal;
                config.Interceptors.Add<ExceptionInterceptor>();
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCisHealthChecks();

                endpoints.MapGrpcService<CIS.InternalServices.Notification.Mailing.Services.MailingService>();
            });
        }
    }
}
