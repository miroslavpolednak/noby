﻿using Microsoft.Extensions.DependencyInjection;
using CIS.Core.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using CIS.Infrastructure.Caching.Configuration;

namespace CIS.Infrastructure.StartupExtensions;

public static class DistributedCachingStartupExtensions
{
    internal static ICisDistributedCacheConfiguration Configuration { get; private set; } = null!;

    public static WebApplicationBuilder AddCisDistributedCache(this WebApplicationBuilder builder)
    {
        // pridat configuration
        Configuration = builder.Configuration
            .GetSection(CisDistributedCacheConfigurationKey)
            .Get<CisDistributedCacheConfiguration>()
            ?? throw new Core.Exceptions.CisConfigurationNotFound(CisDistributedCacheConfigurationKey);

        builder.Services.AddSingleton(Configuration);

        return Configuration.CacheType switch
        {
            ICisDistributedCacheConfiguration.CacheTypes.Redis => registerRedis(builder, Configuration),
            ICisDistributedCacheConfiguration.CacheTypes.InMemory => registerMemory(builder),
            ICisDistributedCacheConfiguration.CacheTypes.MsSql => registerMsSql(builder),
            ICisDistributedCacheConfiguration.CacheTypes.None => registerNone(builder),
            _ => throw new Core.Exceptions.CisConfigurationException(0, "ICisDistributedCacheConfiguration.CacheType is not valid")
        };
    }

    private static WebApplicationBuilder registerNone(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDistributedCache, Caching.MockDistributedCache>();
        return builder;
    }

    private static WebApplicationBuilder registerRedis(
        WebApplicationBuilder builder, 
        ICisDistributedCacheConfiguration distributedCacheConfiguration)
    {
        var cs = getConnectionString(builder);
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = cs;
            options.InstanceName = distributedCacheConfiguration.KeyPrefix + ":";
        });

        return builder;
    }

    private static WebApplicationBuilder registerMsSql(WebApplicationBuilder builder)
    {
        var cs = getConnectionString(builder);
        builder.Services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = cs;
            options.SchemaName = "dbo";
            options.TableName = MsSqlCacheTableName;
        });

        return builder;
    }

    private static WebApplicationBuilder registerMemory(WebApplicationBuilder builder)
    {
        // tady se nezohledni nastaveni prefixu
        // pokud bysme to potrebovali, bude se muset MemoryDistributedCache zabalito do wrapperu, ktery takovou funkcionalitu zajisti
        var options = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());

        builder.Services.AddSingleton<IDistributedCache>(new MemoryDistributedCache(options));

        return builder;
    }

    /// <summary>
    /// Vraci connection string pro cache nebo vyhodi vyjimku
    /// </summary>
    private static string getConnectionString(WebApplicationBuilder builder)
    {
        var cs = builder.Configuration.GetConnectionString(CacheConnectionStringKey);
        if (string.IsNullOrEmpty(cs)) 
        {
            throw new Core.Exceptions.CisConfigurationNotFound($"{CacheConnectionStringKey} connection string not found");
        }
        return cs;
    }

    /// <summary>
    /// Nazev tabulky na mssql ve ktere je ulozena kes
    /// </summary>
    private const string MsSqlCacheTableName = "AppCache";

    /// <summary>
    /// Nazev sekce v appsettings.json kde je konfigurace kese
    /// </summary>
    private const string CisDistributedCacheConfigurationKey = "CisDistributedCache";

    /// <summary>
    /// Nazev connection stringu v appsettings.json kde je CS na redis nebo mssql
    /// </summary>
    private const string CacheConnectionStringKey = "cisDistributedCache";
}
