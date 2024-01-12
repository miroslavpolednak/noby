﻿using CIS.Core;
using CIS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

    public static IServiceCollection AddDapper<TConnectionProvider>(this IServiceCollection services, Func<IServiceProvider, CIS.Core.Data.IConnectionProvider<TConnectionProvider>> implementationFactory)
    {
        services.AddSingleton(implementationFactory);
        return services;
    }

    public static IServiceCollection AddDapperOracle<TConnectionProvider>(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<CIS.Core.Data.IConnectionProvider<TConnectionProvider>>(new OracleConnectionProvider<TConnectionProvider>(connectionString));

        return services;
    }
    /// <summary>
    /// Registrace DbContextu pro EntityFramework
    /// </summary>
    public static WebApplicationBuilder AddEntityFramework<TDbContext>(this WebApplicationBuilder builder, bool enableSensitiveDataLogging = true, string connectionStringKey = CisGlobalConstants.DefaultConnectionStringKey, CisEntityFrameworkOptions<TDbContext>? cisOptions = null)
        where TDbContext : DbContext
    {
        // add custom CIS options
        builder.Services.TryAddSingleton(cisOptions ?? new CisEntityFrameworkOptions<TDbContext>());

        builder.Services.TryAddScoped<BaseDbContextAggregate<TDbContext>>();
        
        // add DbContext
        string? connectionString = builder.Configuration.GetConnectionString(connectionStringKey);
        if (!string.IsNullOrEmpty(connectionString))
        {
            builder.Services
                .AddDbContext<TDbContext>(options => options
                    .UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure())
                    .EnableSensitiveDataLogging(enableSensitiveDataLogging)
                    , ServiceLifetime.Scoped, ServiceLifetime.Singleton);
        }

        return builder;
    }

    public static WebApplicationBuilder AddBaseEntityFramework<TDbContext>(this WebApplicationBuilder builder, bool enableSensitiveDataLogging = true, string connectionStringKey = CisGlobalConstants.DefaultConnectionStringKey, CisEntityFrameworkOptions<TDbContext>? cisOptions = null)
        where TDbContext : DbContext
    {
        // add custom CIS options
        builder.Services.TryAddSingleton(cisOptions ?? new CisEntityFrameworkOptions<TDbContext>());

        // add DbContext
        string? connectionString = builder.Configuration.GetConnectionString(connectionStringKey);
        if (!string.IsNullOrEmpty(connectionString))
        {
            builder.Services
                .AddDbContext<TDbContext>(options => options
                    .UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure())
                    .EnableSensitiveDataLogging(enableSensitiveDataLogging)
                    , ServiceLifetime.Scoped, ServiceLifetime.Singleton);
        }

        return builder;
    }
}
