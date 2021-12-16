using CIS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CIS.Infrastructure.StartupExtensions
{
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
}

namespace CIS.Infrastructure.Data
{
    public static class DataExtensions
    {
        public static void RegisterCisTemporalTable<TEntity>(this ModelBuilder modelBuilder) where TEntity : class
        {
            modelBuilder
                .Entity<TEntity>()
                .ToTable(b => b.IsTemporal(x =>
                {
                    x.HasPeriodStart("InsertTime").HasColumnName("InsertTime");
                    x.HasPeriodEnd("ValidTo").HasColumnName("ValidTo");
                }));
        }
    }
}