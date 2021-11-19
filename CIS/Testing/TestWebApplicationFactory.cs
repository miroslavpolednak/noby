using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CIS.Testing;

public class TestWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    public TestWebApplicationFactory()
    {
        ClientOptions.AllowAutoRedirect = false;
        ClientOptions.BaseAddress = new Uri("https://localhost");
    }

    /*protected override IWebHostBuilder CreateWebHostBuilder()
    {
        return WebHost.CreateDefaultBuilder()
            .UseStartup<TEntryPoint>()
            .ConfigureAppConfiguration((context, conf) =>
            {
                var projectDir = Directory.GetCurrentDirectory();

                conf.AddJsonFile(Path.Combine(projectDir, "appsettings.json"), false);
            });
    }*/
}
