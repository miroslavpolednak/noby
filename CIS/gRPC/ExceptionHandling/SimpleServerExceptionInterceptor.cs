using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;

namespace CIS.Infrastructure.gRPC;

public class SimpleServerExceptionInterceptor : Interceptor
{
    private readonly ILogger<SimpleServerExceptionInterceptor> _logger;

    public SimpleServerExceptionInterceptor(ILogger<SimpleServerExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Core.Exceptions.CisNotFoundException e) // entity neexistuje
        {
            var httpContext = context.GetHttpContext();
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            _logger.EntityNotFound(e);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, e.Message, e.ExceptionCode);
        }
        catch (Core.Exceptions.CisAlreadyExistsException e) // entrita jiz existuje
        {
            _logger.EntityAlreadyExist(e);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.AlreadyExists, e.Message, e.ExceptionCode);
        }
        catch (Core.Exceptions.BaseCisException e)
        {
            var httpContext = context.GetHttpContext();
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            throw GrpcExceptionHelpers.CreateRpcException(e);
        }
        catch (Core.Exceptions.BaseCisArgumentException e)
        {
            var httpContext = context.GetHttpContext();
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            throw GrpcExceptionHelpers.CreateRpcException(e);
        }
        catch (Exception e) when (e is not RpcException) // neosetrena vyjimka
        {
            var httpContext = context.GetHttpContext();
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            _logger.GeneralException(e);
            throw new RpcException(new Status(StatusCode.Internal, e.Message, e), e.Message);
        }
    }
}
