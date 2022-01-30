using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace DomainServices.CaseService.Abstraction;

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
            _logger.LogError(ex, "CaseService unavailable");
            throw new ServiceUnavailableException("CaseService", methodFullName, ex.Message);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition) // nedostupna sluzba EAS atd.
        {
            _logger.LogError(ex, "Some of underlying services are not available or failed to call");
            throw new ServiceUnavailableException("CaseService/dependant_service", methodFullName, ex.Message);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            throw new CisNotFoundException(ex.GetExceptionCodeFromTrailers(), ex.GetErrorMessageFromRpcException());
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
        {
            throw new CisAlreadyExistsException(ex.GetExceptionCodeFromTrailers(), ex.GetErrorMessageFromRpcException());
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.InvalidArgument)
        {
            _logger.LogError("Error: {message}: {code}", ex.GetErrorMessageFromRpcException(), ex.GetArgumentFromTrailers());
            throw new CisArgumentException(ex.GetExceptionCodeFromTrailers(), ex.GetErrorMessageFromRpcException(), ex.GetArgumentFromTrailers());
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, $"Uncought RpcException: {ex.Message}");
            throw new Exception($"RPC Exception: {ex.Message}");
        }
        catch (Exception ex) when (ex is not RpcException)
        {
            _logger.LogError(ex, $"[{methodFullName}] {ex.Message}");
            throw;
        }
    }
}
