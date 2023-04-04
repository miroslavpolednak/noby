using CIS.Core.Configuration;
using CIS.Infrastructure.Configuration;
using DomainServices.DocumentArchiveService.Api.Database.Repositories;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using System.Net.Http.Headers;

namespace DomainServices.DocumentArchiveService.Tests.IntegrationTests.Helpers;

public class GrpcTestFixture<TStartup> : WebApplicationFactory<TStartup>where TStartup : class
{
    internal GrpcChannel GrpcChannel { get; }

    internal IDocumentSequenceRepository DocumentSequenceRepository { get; }

    public GrpcTestFixture()
    {
        DocumentSequenceRepository = Substitute.For<IDocumentSequenceRepository>();

        var client = CreateDefaultClient(new ResponseVersionHandler());

        SetAuthenticationHeader(client);

        GrpcChannel = GrpcChannel.ForAddress(client.BaseAddress!, new GrpcChannelOptions
        {
            HttpClient = client
        });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(nameof(builder));

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.Testing.json", optional: false);


        }).ConfigureServices(services =>
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var cisEnvConfiguration = config.GetSection(CIS.Infrastructure.StartupExtensions.CisEnvironmentConfiguration.JsonConfigurationKey)
                          .Get<CisEnvironmentConfiguration>();

            services.RemoveAll<ICisEnvironmentConfiguration>().AddSingleton<ICisEnvironmentConfiguration>(cisEnvConfiguration!);
            
            // fake logger
            services.RemoveAll<ILoggerFactory>().AddSingleton<ILoggerFactory, NullLoggerFactory>();

            services.RemoveAll<IDocumentSequenceRepository>().AddSingleton(DocumentSequenceRepository);
        });
    }

    private void SetAuthenticationHeader(HttpClient client)
    {
        var config = Services.GetRequiredService<ICisEnvironmentConfiguration>();

        var login = config.InternalServicesLogin;
        var password = config.InternalServicePassword;

        var authenticationString = $"{login}:{password}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
    }

    private class ResponseVersionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            response.Version = request.Version;
            return response;
        }
    }
}
