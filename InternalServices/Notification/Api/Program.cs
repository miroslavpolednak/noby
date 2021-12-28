using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using CIS.Infrastructure.StartupExtensions;

try
{
    var host = Host
        .CreateDefaultBuilder(args)
        .AddCisLogging()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder
                .UseStartup<CIS.InternalServices.Notification.Api.Startup>()
                .CaptureStartupErrors(true);
        });

    await host.Build().RunAsync();
}
finally
{
    CisLogging.CloseAndFlushLog();
}
