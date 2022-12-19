using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Infrastructure.gRPC;

public static class GrpcExtensions
{
    public static IApplicationBuilder UseGrpc2WebApiException(this IApplicationBuilder app)
        => app.UseWhen(context => context.Request.ContentType == "application/json", builder => app.UseMiddleware<Middleware.Grpc2WebApiExceptionMiddleware>());

    public static IHttpClientBuilder CisConfigureChannelWithoutCertificateValidation(this IHttpClientBuilder builder)
        => builder.ConfigureChannel(configureChannel);

    public static IHttpClientBuilder AddCisCallCredentials(this IHttpClientBuilder builder)
        => builder.AddCallCredentials(addCredentials);

    private static Func<AuthInterceptorContext, Metadata, IServiceProvider, Task> addCredentials =
        (context, metadata, serviceProvider) =>
        {
            var configuration = serviceProvider.GetRequiredService<Core.Configuration.ICisEnvironmentConfiguration>();

            if (string.IsNullOrEmpty(configuration.InternalServicesLogin) || string.IsNullOrEmpty(configuration.InternalServicePassword))
                throw new System.Security.Authentication.InvalidCredentialException("InternalServicesLogin or InternalServicePassword is empty");
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{configuration.InternalServicesLogin}:{configuration.InternalServicePassword}");

            // add authentication header
            metadata.Add("Authorization", $"Basic {Convert.ToBase64String(plainTextBytes)}");

            return Task.CompletedTask;
        };

    private static Action<GrpcChannelOptions> configureChannel =
        (GrpcChannelOptions options) =>
        {
            HttpClientHandler httpHandler = new()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            options.HttpHandler = httpHandler;
        };
}