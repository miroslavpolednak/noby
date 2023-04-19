using CIS.Core.Configuration;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Http.Headers;
using Grpc.Core.Interceptors;
using CIS.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CIS.Testing;
public class WebApplicationFactoryFixture<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private Action<IServiceCollection>? _configureServices = null;
    private Action<WebHostBuilderContext, IConfigurationBuilder>? _configureAppConfiguration = null;

    public CisWebApplicationFactoryOptions CisWebFactoryConfiguration { get; set; } = new();

    private HttpClient? _httpClient;
    public HttpClient HttpClient => _httpClient ??= CreateHttpClient();

    private GrpcChannel? _channel;
    public GrpcChannel Channel => _channel ??= GrpcChannel.ForAddress(HttpClient.BaseAddress!, new GrpcChannelOptions
    {
        HttpClient = HttpClient
    });

    public WebApplicationFactoryFixture()
    {
    }

    public TService CreateGrpcClient<TService>() where TService : ClientBase<TService>
    {
        var exceptionInterceptor = Services.GetRequiredService<GenericClientExceptionInterceptor>();
        var userForwardingInterceptor = Services.GetRequiredService<ContextUserForwardingClientInterceptor>();
        var invoker = Channel.Intercept(exceptionInterceptor, userForwardingInterceptor);
        return (TService)Activator.CreateInstance(typeof(TService), new object[] { invoker })!;
    }


    public WebApplicationFactoryFixture<TStartup> ConfigureCisTestOptions(Action<CisWebApplicationFactoryOptions>? options)
    {
        options?.Invoke(CisWebFactoryConfiguration);
        return this;
    }

    public WebApplicationFactoryFixture<TStartup> ConfigureServices(Action<IServiceCollection> configureServices)
    {
        _configureServices = configureServices;
        return this;
    }

    public WebApplicationFactoryFixture<TStartup> ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configuration)
    {
        _configureAppConfiguration = configuration;
        return this;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(nameof(builder));

        builder.ConfigureAppConfiguration((context, config) =>
        {
            if (CisWebFactoryConfiguration.UseTestAppsettings)
                config.SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile(CisWebFactoryConfiguration.AppsettingsName, optional: false);

            _configureAppConfiguration?.Invoke(context, config);
        }).
        ConfigureServices(services =>
        {
            // Mock of service discovery and other things in CisEnvironmentConfiguration
            if (CisWebFactoryConfiguration.UseMockCisEnvironmentConfiguration)
            {
                var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                var cisEnvConfiguration = config.GetSection(CIS.Infrastructure.StartupExtensions.CisEnvironmentConfiguration.JsonConfigurationKey)
                              .Get<CisEnvironmentConfiguration>();

                services.RemoveAll<ICisEnvironmentConfiguration>().AddSingleton<ICisEnvironmentConfiguration>(cisEnvConfiguration!);
            }
           
            // fake logger
            if (CisWebFactoryConfiguration.UseNullLogger)
                services.RemoveAll<ILoggerFactory>().AddSingleton<ILoggerFactory, NullLoggerFactory>();

            if (CisWebFactoryConfiguration.UseDbContextAutoMock)
                AutoMockDbContexts(services);


            _configureServices?.Invoke(services);
        });
    }

    private static void AutoMockDbContexts(IServiceCollection services)
    {
        var dbContextTypes = typeof(TStartup).Assembly.GetTypes().Where(t => typeof(DbContext).IsAssignableFrom(t));
        var addDbContextMethod = typeof(EntityFrameworkServiceCollectionExtensions).GetMethods().Where(i => i.Name == "AddDbContext" && i.IsGenericMethod == true).First();

        foreach (var dbContextType in dbContextTypes)
        {
            // Remove existing dbContext(real db) 
            var dbContextOptions = typeof(DbContextOptions<>).MakeGenericType(dbContextType);
            var dbContextDescriptor = services.SingleOrDefault(
                                    d => d.ServiceType == dbContextOptions);

            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            //Dynamically register db context with in memory db
            var dbName = Guid.NewGuid().ToString(); // db is unique per test class 
            var addDbContextGenericMethod = addDbContextMethod.MakeGenericMethod(dbContextType);
            var optionsAction = new Action<DbContextOptionsBuilder>(options =>
            {
                options.UseInMemoryDatabase(dbName);
                options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            // Create AddDbContext parameters
            object[] parametersArray = new object[] { services, optionsAction, ServiceLifetime.Scoped, ServiceLifetime.Scoped };
            // Call AddDbContext with parameters
            addDbContextGenericMethod?.Invoke(services, parametersArray);
        }
    }

    private HttpClient CreateHttpClient()
    {
        var client = CreateDefaultClient();

        if (CisWebFactoryConfiguration.UseNobyAuthenticationHeader)
            SetAuthHeader(client);

        if (CisWebFactoryConfiguration.Header is not null && CisWebFactoryConfiguration.Header.Any())
            SetCustomHeader(client, CisWebFactoryConfiguration.Header);

        return client;
    }

    private void SetCustomHeader(HttpClient client, Dictionary<string, string?> header)
    {
        foreach (var headerItem in header)
        {
            if (!client.DefaultRequestHeaders.Contains(headerItem.Key))
                client.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
        }
    }

    private void SetAuthHeader(HttpClient client)
    {
        var config = Services.GetRequiredService<ICisEnvironmentConfiguration>();
        var authenticationString = $"{config.InternalServicesLogin}:{config.InternalServicePassword}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));
        // add noby specific header 
        client.DefaultRequestHeaders.Add("noby-user-id", "3048");
        client.DefaultRequestHeaders.Add("noby-user-ident", "KBUID=A09FK3");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
    }
}
