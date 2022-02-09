using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.Logging;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace DomainServices.SalesArrangementService.Abstraction;

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
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable) // nedostupna sluzba
        {
            _logger.ServiceUnavailable("SalesArrangementService", ex);
            throw new ServiceUnavailableException("SalesArrangementService", methodFullName, ex.Message);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            throw new CisNotFoundException(ex.GetExceptionCodeFromTrailers(), ex.GetErrorMessageFromRpcException());
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.InvalidArgument)
        {
            int code = ex.GetExceptionCodeFromTrailers();
            string arg = ex.GetArgumentFromTrailers() ?? "";
            string message = ex.GetErrorMessageFromRpcException();

            _logger.InvalidArgument(code, arg, message, ex);
            throw new CisArgumentException(code, message, arg);
        }
        catch (Exception ex) when (ex is not RpcException)
        {
            _logger.GeneralException(methodFullName, ex.Message, ex);
            throw;
        }
    }
}
