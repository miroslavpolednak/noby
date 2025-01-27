﻿using CIS.Core.Configuration;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Http.Headers;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Hosting;

namespace CIS.Testing;

public class WebApplicationFactoryFixture<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    private Action<IServiceCollection>? _configureServices;
    private Action<HostBuilderContext, IConfigurationBuilder>? _configureAppConfiguration;

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

    public TService CreateGrpcClient<TService>(bool addClientExceptionInterceptor = false)
        where TService : ClientBase<TService>
    {
        List<Interceptor> interceptors = new();

        var userForwardingInterceptor = Services.GetService<ContextUserForwardingClientInterceptor>();
        if (userForwardingInterceptor != null)
        {
            interceptors.Add(userForwardingInterceptor);
        }

        if (addClientExceptionInterceptor)
        {
            interceptors.Add(Services.GetRequiredService<GenericClientExceptionInterceptor>());
        }

        if (interceptors.Count != 0)
        {
            var invoker = Channel.Intercept(interceptors.ToArray());
            return (TService)Activator.CreateInstance(typeof(TService), new object[] { invoker })!;
        }
        else
        {
            return (TService)Activator.CreateInstance(typeof(TService), new object[] { Channel })!;
        }
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

    public WebApplicationFactoryFixture<TStartup> ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configuration)
    {
        _configureAppConfiguration = configuration;
        return this;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(nameof(builder));

        builder
            .ConfigureHostConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile(CisWebFactoryConfiguration.AppSettingsName, optional: false);
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                _configureAppConfiguration?.Invoke(context, config);
            })
            .ConfigureServices(services =>
            {
                // fake logger
                if (CisWebFactoryConfiguration.UseNullLogger)
                {
                    services.RemoveAll<ILoggerFactory>().AddSingleton<ILoggerFactory, NullLoggerFactory>();
                }

                if (CisWebFactoryConfiguration.UseDbContextAutoMock)
                {
                    CisWebFactoryConfiguration.DbMockAdapter.MockDatabase<TStartup>(services);
                }

                _configureServices?.Invoke(services);
            });

        builder.UseEnvironment("Testing");

        return base.CreateHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        CisWebFactoryConfiguration.DbMockAdapter.Dispose(disposing);
    }

    private HttpClient CreateHttpClient()
    {
        var client = CreateDefaultClient();

        if (CisWebFactoryConfiguration.UseNobyAuthenticationHeader)
            SetAuthHeader(client);

        if (CisWebFactoryConfiguration.Header is not null && CisWebFactoryConfiguration.Header.Count != 0)
            SetCustomHeader(client, CisWebFactoryConfiguration.Header);

        return client;
    }

    private static void SetCustomHeader(HttpClient client, Dictionary<string, string?> header)
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
        client.DefaultRequestHeaders.Add("noby-user-id", Constants.HeaderServiceContextUserId.ToString(System.Globalization.CultureInfo.InvariantCulture));
        client.DefaultRequestHeaders.Add("noby-user-ident", Constants.HeaderServiceContextIdent);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
    }
}
