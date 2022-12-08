using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;
using CIS.Core.Exceptions;

namespace CIS.Infrastructure.gRPC;

/// <summary>
/// Server Interceptor který odchytává vyjímky v doménové službě a vytváří z nich RpcException, které dokáže Clients projekt zase přetavit na původní CIS exception.
/// </summary>
public sealed class GenericServerExceptionInterceptor 
    : Interceptor
{
    private readonly ILogger<GenericServerExceptionInterceptor> _logger;

    public GenericServerExceptionInterceptor(ILogger<GenericServerExceptionInterceptor> logger)
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
        // DS neni dostupna
        catch (CisServiceUnavailableException ex)
        {
            _logger.ExtServiceUnavailable(ex.ServiceName, ex);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, $"Service '{ex.ServiceName}' unavailable", 0);
        }
        // 500 z volane externi sluzby
        catch (CisServiceServerErrorException ex)
        {
            setHttpStatus(StatusCodes.Status424FailedDependency);
            _logger.ExtServiceUnavailable(ex.ServiceName, ex);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.FailedPrecondition, $"Service '{ex.ServiceName}' failed with HTTP 500", 0);
        }
        catch (Core.Exceptions.CisNotFoundException e) // entity neexistuje
        {
            setHttpStatus(StatusCodes.Status404NotFound);
            _logger.EntityNotFound(e);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, e.Message, e.ExceptionCode);
        }
        catch (Core.Exceptions.CisAlreadyExistsException e) // entita jiz existuje
        {
            setHttpStatus(StatusCodes.Status400BadRequest);
            _logger.EntityAlreadyExist(e);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.AlreadyExists, e.Message, e.ExceptionCode);
        }
        catch (Core.Exceptions.CisServiceCallResultErrorException e)
        {
            setHttpStatus(StatusCodes.Status400BadRequest);
            throw GrpcExceptionHelpers.CreateRpcExceptionFromServiceCall(e);
        }
        catch (Core.Exceptions.CisValidationException e)
        {
            setHttpStatus(StatusCodes.Status400BadRequest);
            var collection = new GrpcErrorCollection(e.Errors!.Select(t => new GrpcErrorCollection.GrpcErrorCollectionItem(t.Key, t.Message)));
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, e.Message, collection);
        }
        catch (Core.Exceptions.BaseCisException e)
        {
            setHttpStatus(StatusCodes.Status400BadRequest);
            throw GrpcExceptionHelpers.CreateRpcException(e);
        }
        catch (Core.Exceptions.BaseCisArgumentException e)
        {
            setHttpStatus(StatusCodes.Status400BadRequest);
            throw GrpcExceptionHelpers.CreateRpcException(e);
        }
        catch (Exception e) when (e is not RpcException) // neosetrena vyjimka
        {
            setHttpStatus(StatusCodes.Status500InternalServerError);
            _logger.ServerUncoughtRpcException(e);
            throw new RpcException(new Status(StatusCode.Internal, e.Message, e), e.Message);
        }

        void setHttpStatus(int code)
        {
            var httpContext = context.GetHttpContext();
            httpContext.Response.StatusCode = code;
        }
    }
}
