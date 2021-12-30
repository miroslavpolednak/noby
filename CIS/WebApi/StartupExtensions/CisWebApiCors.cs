namespace CIS.Infrastructure.StartupExtensions;

public static class CisWebApiCors
{
    public static WebApplicationBuilder AddCisWebApiCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(x =>
        {
            x.AddPolicy("allowany",
                policyBuilder => policyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return builder;
    }
}
