using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;

namespace DomainServices.CaseService.Clients;

internal class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, 
        ClientInterceptorContext<TRequest, TResponse> context, 
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var response = continuation(request, context);
        var task = catcher(response, context.Method.FullName);
        return new AsyncUnaryCall<TResponse>(task, response.ResponseHeadersAsync, response.GetStatus, response.GetTrailers, response.Dispose);
    }

    private async Task<TResponse> catcher<TResponse>(AsyncUnaryCall<TResponse> responseAsync, string methodFullName)
    {
        try
        {
            return await responseAsync;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition) // nedostupna sluzba EAS atd.
        {
            _logger.ExtServiceUnavailable("CaseService", ex);
            throw new CisServiceUnavailableException("CaseService/dependant_service", methodFullName, ex.Message);
        }
    }
}
