using CIS.Core.Configuration;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Configuration;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;

public class WebApplicationFactoryFixture<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private Action<IServiceCollection>? _configureServices = null;
    private Action<WebHostBuilderContext, IConfigurationBuilder>? _configureAppConfiguration = null;

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
            config.SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.Testing.json", optional: false);

            _configureAppConfiguration?.Invoke(context, config);
        }).
        ConfigureServices(services =>
        {
            // Mock of service discovery
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var cisEnvConfiguration = config.GetSection(CIS.Infrastructure.StartupExtensions.CisEnvironmentConfiguration.JsonConfigurationKey)
                          .Get<CisEnvironmentConfiguration>();

            services.RemoveAll<ICisEnvironmentConfiguration>().AddSingleton<ICisEnvironmentConfiguration>(cisEnvConfiguration!);

            // fake logger
            services.RemoveAll<ILoggerFactory>().AddSingleton<ILoggerFactory, NullLoggerFactory>();

            _configureServices?.Invoke(services);
        });
    }

    private HttpClient CreateHttpClient()
    {
        var client = CreateDefaultClient();
        SetAuthHeader(client);
        return client;
    }

    private void SetAuthHeader(HttpClient client)
    {
        var config = Services.GetRequiredService<ICisEnvironmentConfiguration>();
        var authenticationString = $"{config.InternalServicesLogin}:{config.InternalServicePassword}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));
        // added noby specific header 
        client.DefaultRequestHeaders.Add("noby-user-id", "3048");
        client.DefaultRequestHeaders.Add("noby-user-ident", "KBUID=A09FK3");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
    }
}
