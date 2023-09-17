using SharedComponents.GrpcServiceBuilderHelpers;
using CIS.Infrastructure.StartupExtensions;
using Microsoft.AspNetCore.Builder;

namespace SharedComponents;

public static class GrpcServiceBuilder
{
    public static IGrpcServiceFluentBuilder CreateGrpcService(string[] args, Type mainAssembly)
    {
        bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc", StringComparison.OrdinalIgnoreCase));

        //TODO workaround until .NET6 UseWindowsService() will work with WebApplication
        var webAppOptions = runAsWinSvc
            ?
            new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
            :
            new WebApplicationOptions { Args = args };
        var builder = WebApplication.CreateBuilder(webAppOptions);

        // globalni nastaveni prostredi
        var configuration = builder.AddCisEnvironmentConfiguration();

        var settings = new GrpcServiceBuilderSettings
        {
            MainAssembly = mainAssembly,
            RunAsWindowsService = runAsWinSvc,
            Builder = builder,
            EnvironmentConfiguration = configuration
        };

        return new GrpcServiceFluentBuilder(settings);
    }
}
