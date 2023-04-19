namespace MPSS.Security.Noby;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddMpssSecurityCookie(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetRequiredSection("MpssSecurityCookie").Get<Configuration>();
        builder.Services.AddSingleton(configuration!);

        builder.Services.AddTransient<IPortal, Portal>();

        return builder;
    }
}
