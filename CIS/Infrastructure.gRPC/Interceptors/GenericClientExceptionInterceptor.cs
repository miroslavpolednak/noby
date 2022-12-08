using CIS.Core.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;

namespace CIS.Infrastructure.gRPC;

/// <summary>
/// Client Interceptor pro konverzi RpcException na CIS vyjímky.
/// </summary>
/// <remarks>
/// Používáme, abychom chyby z doménových služeb přetavili z generické RpcException na konkrétní vyjímky, které vyhodila daná doménová služba.
/// </remarks>
public sealed class GenericClientExceptionInterceptor
    : Interceptor
{
    private readonly ILogger<GenericClientExceptionInterceptor> _logger;

    public GenericClientExceptionInterceptor(ILogger<GenericClientExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var response = continuation(request, context);
        var task = catcher(response, context.Method.ServiceName, context.Method.FullName);
        return new AsyncUnaryCall<TResponse>(task, response.ResponseHeadersAsync, response.GetStatus, response.GetTrailers, response.Dispose);
    }

    private async Task<TResponse> catcher<TResponse>(AsyncUnaryCall<TResponse> responseAsync, string serviceName, string methodFullName)
    {
        try
        {
            return await responseAsync;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable) // nedostupna sluzba
        {
            _logger.ServiceUnavailable("GRPC service unavailable", ex);
            throw new CisServiceUnavailableException(serviceName, methodFullName, ex.Message);
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
            // try list of errors first
            var messages = ex.GetErrorMessagesFromRpcException();
            if (messages.Any()) // most likely its validation exception
            {
                throw new CisValidationException(messages);
            }
            else // its single argument exception
            {
                int code = ex.GetExceptionCodeFromTrailers();
                string arg = ex.GetArgumentFromTrailers() ?? "";
                string message = ex.GetErrorMessageFromRpcException();

                _logger.ClientInvalidArgument(code, arg, ex);
                throw new CisArgumentException(code, message, arg);
            }
        }
        catch (RpcException ex)
        {
            _logger.ClientUncoughtRpcException(methodFullName, ex);
            throw;
        }
    }
}
