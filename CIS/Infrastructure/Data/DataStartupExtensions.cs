using CIS.Infrastructure.Data;

namespace CIS.Infrastructure.StartupExtensions;

public static class DataStartupExtensions
{
    public static IServiceCollection AddDapper(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<CIS.Core.Data.IConnectionProvider>(new SqlConnectionProvider(connectionString));

        return services;
    }

    public static IServiceCollection AddDapper<TConnectionProvider>(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<CIS.Core.Data.IConnectionProvider<TConnectionProvider>>(new SqlConnectionProvider<TConnectionProvider>(connectionString));

        return services;
    }
}
