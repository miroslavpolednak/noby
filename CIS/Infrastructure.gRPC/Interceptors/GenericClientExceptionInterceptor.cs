using CIS.Core.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using CIS.Infrastructure.Logging;
using CIS.Core.Exceptions.ExternalServices;
using Google.Rpc;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

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
        // DS neni dostupna
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable) // nedostupna sluzba
        {
            _logger.ServiceUnavailable("GRPC service unavailable", ex);
            throw new CisServiceUnavailableException(serviceName, methodFullName, ex.Message);
        }
        // 403
        catch (RpcException ex) when (ex.StatusCode == StatusCode.PermissionDenied)
        {
            var detail = ex.GetRpcStatus()?.GetDetail<PreconditionFailure>();
            throw new CisAuthorizationException(ex.Message, detail?.Violations?.FirstOrDefault()?.Subject);
        }
        // entity neexistuje
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            var detail = ex.GetRpcStatus()?.GetDetail<ResourceInfo>();
            _ = int.TryParse(detail?.Description, out int exceptionCode);
            throw new CisNotFoundException(exceptionCode, detail?.ResourceType ?? "", detail?.ResourceName ?? "");
        }
        // entita jiz existuje
        catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
        {
            var detail = ex.GetRpcStatus()?.GetDetail<ResourceInfo>();
            _ = int.TryParse(detail?.Description, out int exceptionCode);
            throw new CisAlreadyExistsException(exceptionCode, detail?.ResourceType ?? "", detail?.ResourceName ?? "");
        }
        // validacni chyby vyvolane validatorem nebo rucne
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.InvalidArgument)
        {
            var detail = ex.GetRpcStatus()?.GetDetail<BadRequest>();
            if (detail?.FieldViolations.Any() ?? false)
            {
                throw new CisValidationException(detail.FieldViolations.Select(t => new CisExceptionItem(t.Field, t.Description)));
            }
            else
            {
                throw new CisValidationException(ex.Message);
            }
        }
        // externi sluzba neni dostupna nebo vratila 500
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Aborted)
        {
            var detailResource = ex.GetRpcStatus()?.GetDetail<ResourceInfo>();
            var detailInfo = ex.GetRpcStatus()?.GetDetail<ErrorInfo>();
            _ = int.TryParse(detailInfo?.Reason, out int exceptionCode);
            _ = int.TryParse(detailResource?.Description, out int internalCode);

            // externi sluzba vratila http 500
            if (internalCode == CisExternalServiceServerErrorException.DefaultExceptionCode)
            {
                throw new CisExternalServiceUnavailableException(exceptionCode, detailResource?.ResourceName ?? "");
            }
            else
            {
                throw new CisExternalServiceUnavailableException(exceptionCode, detailResource?.ResourceName ?? "");
            }
        }
        catch (RpcException ex) when (ex.Trailers != null && ex.StatusCode == StatusCode.Unknown)
        {
            var detail = ex.GetRpcStatus()?.GetDetail<ErrorInfo>();
            throw new CisValidationException(detail?.Domain ?? "", detail?.Reason ?? "");
        }
        catch (RpcException ex)
        {
            _logger.ClientUncoughtRpcException(methodFullName, ex);
            throw;
        }
    }
}
