using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;
using CIS.Core.Exceptions;
using CIS.Core.Exceptions.ExternalServices;
using Google.Rpc;
using Google.Protobuf.WellKnownTypes;

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
            _logger.ExternalServiceUnavailable(ex.ServiceName, ex);
            throw createRpcExceptionWithPreconditionFailure(Code.Unavailable, $"Service '{ex.ServiceName}' unavailable", cViolation("ServiceUnavailable", ex.ServiceName, ex.Message));
        }
        // 403
        catch (CisAuthorizationException ex)
        {
            setHttpStatus(StatusCodes.Status403Forbidden);
            _logger.ServiceAuthorizationFailed(ex);
            throw createRpcExceptionWithPreconditionFailure(Code.PermissionDenied, "Service authorization failed: " + ex.Message, cViolation("PermissionDenied", ex.Username ?? "", ex.Message));
        }
        // 500 z volane externi sluzby
        catch (CisServiceServerErrorException ex)
        {
            setHttpStatus(StatusCodes.Status500InternalServerError);
            _logger.ExternalServiceUnavailable(ex.ServiceName, ex);
            throw createRpcExceptionWithPreconditionFailure(Code.Internal, $"Service '{ex.ServiceName}' failed with HTTP 500", cViolation("ServiceUnavailable", ex.ServiceName, ex.Message));
        }
        catch (CisNotFoundException e) // entity neexistuje
        {
            setHttpStatus(StatusCodes.Status404NotFound);
            _logger.EntityNotFound(e);
            throw createRpcExceptionWithResourceInfo(Code.NotFound, e.Message, e.EntityName, e.EntityId, e.ExceptionCode);
        }
        catch (CisAlreadyExistsException e) // entita jiz existuje
        {
            setHttpStatus(StatusCodes.Status400BadRequest);
            _logger.EntityAlreadyExist(e);
            throw createRpcExceptionWithResourceInfo(Code.AlreadyExists, e.Message, e.EntityName, e.EntityId, e.ExceptionCode);
        }
        // validacni chyby vyvolane validatorem nebo rucne
        catch (CisValidationException e)
        {
            setHttpStatus(StatusCodes.Status400BadRequest);
            _logger.LogValidationResults(e);

            var details = new BadRequest();
            if (e.Errors?.Any() ?? false)
            {
                details.FieldViolations.AddRange(e.Errors.Select(t => new BadRequest.Types.FieldViolation { Field = t.ExceptionCode, Description = t.Message }));
            }

            throw (new Google.Rpc.Status
            {
                Code = (int)Code.InvalidArgument,
                Message = e.Message,
                Details = { Any.Pack(details) }
            }).ToRpcException();
        }
        // externi sluzba neni dostupna
        catch (CisExternalServiceUnavailableException e)
        {
            setHttpStatus(StatusCodes.Status500InternalServerError);
            _logger.ExternalServiceUnavailable(e.ServiceName, e);
            throw createRpcExceptionForExternalService(e.Message, e.ServiceName, e.ExceptionCode);
        }
        // externi sluzba vratila http 500
        catch (CisExternalServiceServerErrorException e)
        {
            setHttpStatus(StatusCodes.Status500InternalServerError);
            _logger.ExternalServiceServerError(e.ServiceName, e);
            throw createRpcExceptionForExternalService(e.Message, e.ServiceName, e.ExceptionCode);
        }
        catch (BaseCisException e)
        {
            setHttpStatus(StatusCodes.Status400BadRequest);

            throw (new Google.Rpc.Status
            {
                Code = (int)Code.Unknown,
                Message = e.Message,
                Details = { Any.Pack(cCisErrorCode(e.ExceptionCode)) }
            }).ToRpcException();
        }
        catch (Exception e) when (e is not RpcException) // neosetrena vyjimka
        {
            setHttpStatus(StatusCodes.Status500InternalServerError);
            _logger.ServerUncoughtRpcException(e);
            
            throw (new Google.Rpc.Status
            {
                Code = (int)Code.Internal,
                Message = e.Message
            }).ToRpcException();
        }
        catch (Exception e) {
            setHttpStatus(StatusCodes.Status500InternalServerError);
            _logger.ServerUncoughtRpcException(e);

            throw (new Google.Rpc.Status
            {
                Code = (int)Code.Internal,
                Message = e.Message
            }).ToRpcException();
        }

        void setHttpStatus(int code)
        {
            var httpContext = context.GetHttpContext();
            httpContext.Response.StatusCode = code;
        }
    }

    private static ErrorInfo cCisErrorCode(in string exceptionCode)
        => cErrorInfo("cis_error_code", exceptionCode);

    private static ErrorInfo cErrorInfo(in string domain, in string reason)
        => new() { Domain = domain, Reason = reason };

    private static Google.Rpc.PreconditionFailure.Types.Violation cViolation(in string type, in string subject, in string? description)
        => new() { Type = type, Subject = subject, Description = description ?? "" };

    private static ResourceInfo cResourceInfo(in string resourceType, in string resourceName, in string? description = "")
        => new() { ResourceType = resourceType, ResourceName = resourceName, Description = description ?? "" };

    private static RpcException createRpcExceptionWithPreconditionFailure(in Code code, in string message, Google.Rpc.PreconditionFailure.Types.Violation violation)
        => (new Google.Rpc.Status
        {
            Code = (int)code,
            Message = message,
            Details = 
            { 
                Any.Pack(new PreconditionFailure { Violations = { violation } }) 
            }
        }).ToRpcException();
    
    private static RpcException createRpcExceptionWithResourceInfo(in Code code, in string message, in string? entityName, in object? entityId, in string? exceptionCode)
        => (new Google.Rpc.Status
        {
            Code = (int)code,
            Message = message,
            Details =
            {
                Any.Pack(cResourceInfo(entityName ?? "", entityId?.ToString() ?? "", exceptionCode))
            }
        }).ToRpcException();

    private static RpcException createRpcExceptionForExternalService(in string message, in string serviceName, in string exceptionCode)
        => (new Google.Rpc.Status
            {
                Code = (int)Code.Aborted,
                Message = message,
                Details =
                {
                    Any.Pack(cResourceInfo("external_service", serviceName)),
                    Any.Pack(cCisErrorCode(exceptionCode))
                }
            }).ToRpcException();
}
