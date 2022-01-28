using CIS.Core.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace CIS.InternalServices.ServiceDiscovery.Abstraction;

internal class ExceptionInterceptor 
    : Interceptor
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
            _logger.LogError(ex, "ServiceDiscovery service unavailable");
            throw new ServiceUnavailableException("ServiceDiscovery", methodFullName, ex.Message);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            _logger.LogError(ex, $"ServiceDiscovery service not found: {ex.Message}");
            throw new CisNotFoundException(101, ex.Message);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
        {
            int.TryParse(ex.Trailers?.Get(ExceptionHandlingConstants.GrpcTrailerCisCodeKey)?.Value, out int code);
            throw new CisArgumentException(code, ex.Message, ex.Trailers?.Get(ExceptionHandlingConstants.GrpcTrailerCisArgumentKey)?.Value);
        }
        catch (RpcException ex) when (ex.Trailers != null)
        {
            if (int.TryParse(ex.Trailers?.Get(ExceptionHandlingConstants.GrpcTrailerCisCodeKey)?.Value, out int code))
                throw new CisException(code, ex.Message);
            else
            {
                _logger.LogError(ex, $"Uncought RpcException: {ex.Message}");
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $" [{methodFullName}] {ex.Message}");
            throw;
        }
    }
}
