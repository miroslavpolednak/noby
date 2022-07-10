using CIS.Core.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace CIS.InternalServices.Storage.Abstraction;

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
            _logger.LogError(ex, "StorageService unavailable");
            throw new CisServiceUnavailableException("StorageService", methodFullName, ex.Message);
        }
        catch (RpcException ex) when (ex.Trailers != null)
        {
            int.TryParse(ex.Trailers?.Get(ExceptionHandlingConstants.GrpcTrailerCisCodeKey)?.Value, out int code);

            switch (code)
            {
                case 101:
                    throw new CisException(code, $"Storage service not found: {ex.Message}");
                case 201:
                case 202:
                    throw new Exceptions.InvalidBlobKeyException(code, ex.Message);
                case 203:
                    throw new Exceptions.InvalidSessionIdException(code, ex.Message);
                case 204:
                case 205:
                case 206:
                case 208:
                    throw new Exceptions.BlobNotFoundException(code, ex.Message);
                case 207:
                    throw new Exceptions.BlobDataNullException(code, ex.Message);
                case 209:
                    throw new Exceptions.InvalidBlobSessionException(code, ex.Message);
                default:
                    if (ex.StatusCode == StatusCode.InvalidArgument)
                        throw new CisArgumentException(code, ex.Message, ex.Trailers?.Get(ExceptionHandlingConstants.GrpcTrailerCisArgumentKey)?.Value);
                    else
                        throw new CisException(code, ex.Message);
            }
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, $"Uncought RpcException: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[{methodFullName}] {ex.Message}");
            throw;
        }
    }
}
