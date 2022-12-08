using System.Net;
using CIS.Core.Exceptions;
using CIS.Infrastructure.Logging;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CustomerService.Api.Clients;

[TransientService, SelfService]
internal class CustomerManagementHttpHandler<TService> : DelegatingHandler
{
    private const string ServiceName = "CustomerManagement";

    private readonly CustomerManagementErrorMap _errorMap;
    private readonly ILogger<TService> _logger;

    public CustomerManagementHttpHandler(CustomerManagementErrorMap errorMap, ILogger<TService> logger) : base(BuildCertificationValidatorHttpHandler())
    {
        _errorMap = errorMap;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await LogRequest(request, cancellationToken);

        var response = await SendRequest(request, cancellationToken);

        await LogResponse(request, response, cancellationToken);

        if (response.IsSuccessStatusCode)
            return response;

        switch((int)response.StatusCode)
        {
            case (int)HttpStatusCode.NotFound:
                throw new CisNotFoundException(11000, "Customer does not exist.");

            case >= 400 and < 500:
                await ProcessErrorStatusCode(response.StatusCode, response.Content);
                throw new NotImplementedException();

            default:
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, $"CustomerManagement Endpoint '{request.RequestUri}' unavailable", 11002);
        };
    }

    protected virtual async Task ProcessErrorStatusCode(HttpStatusCode statusCode, HttpContent content)
    {
        var error = await content.ReadFromJsonAsync<ErrorModel>();

        if (error is not null)
            _errorMap.ResolveValidationError(error.Code);

        var text = content.ReadAsStringAsync();

        throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"Incorrect inputs to CustomerManagement: {text}", 11003);
    }

    private Task<HttpResponseMessage> SendRequest(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return base.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new CisServiceUnavailableException(ServiceName, request.RequestUri!.ToString(), ex.Message);
        }
    }

    private async Task LogRequest(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content is null)
            return;

        using var _ = BeginLoggerScope(await request.Content.ReadAsStringAsync(cancellationToken));

        _logger.HttpRequestPayload(request);
    }

    private async Task LogResponse(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        using var _ = BeginLoggerScope(await response.Content.ReadAsStringAsync(cancellationToken));

        _logger.HttpResponsePayload(request, (int)response.StatusCode);
    }

    private IDisposable BeginLoggerScope(object scopeObj)
    {
        var state = new Dictionary<string, object>
        {
            { "Payload", scopeObj }
        };

        return _logger.BeginScope(state);
    }

    private static HttpMessageHandler BuildCertificationValidatorHttpHandler()
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = ValidationCallback
        };

        static bool ValidationCallback(HttpRequestMessage arg1, X509Certificate2? arg2, X509Chain? arg3, SslPolicyErrors arg4) => true;
    }

    private class ErrorModel
    {
        public int HttpStatusCode { get; set; }

        public string Code { get; set; } = string.Empty;
    }
}