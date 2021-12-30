using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using CIS.Infrastructure.StartupExtensions;

    var host = Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder
                .UseStartup<CIS.InternalServices.Notification.Api.Startup>()
                .CaptureStartupErrors(true);
        });

    await host.Build().RunAsync();
