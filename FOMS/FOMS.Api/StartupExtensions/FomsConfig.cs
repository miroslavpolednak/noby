namespace FOMS.Api.StartupExtensions;

internal static class FomsConfig
{
    public static Infrastructure.Configuration.AppConfiguration AddFomsConfig(this WebApplicationBuilder builder)
    {
        Infrastructure.Configuration.AppConfiguration appConfiguration = new();

        // bind from config files
        builder.Configuration.Bind("NOBY", appConfiguration);
        // register to DI
        builder.Services.AddSingleton(appConfiguration);

        return appConfiguration;
    }
}
