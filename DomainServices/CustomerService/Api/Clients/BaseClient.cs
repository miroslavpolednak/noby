using System.ServiceModel;
using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CustomerService.Api.Clients;

public abstract class BaseClient<TApiException> where TApiException : Exception
{
    protected readonly HttpClient _httpClient;
    protected readonly ILogger _logger;

    protected BaseClient(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    protected async Task<TResult> CallEndpoint<TResult>(Func<Task<TResult>> func)
    {
        try
        {
            return await func();
        }
        catch (TApiException ex) when (GetApiExceptionStatusCode(ex) == 404)
        {
            _logger.LogError("Customer was not found: {detail}", GetApiExceptionDetail(ex));

            throw new CisNotFoundException(99999, "Customer not found"); //TODO: ErrorCode
        }
        catch (TApiException ex) when (GetApiExceptionStatusCode(ex) != 500)
        {
            _logger.LogError("Incorrect inputs to CustomerManagement: {detail}", GetApiExceptionDetail(ex));

            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"Incorrect inputs to CustomerManagement: {GetApiExceptionDetail(ex)}", 99999); //TODO> ErrorCode
        }
        catch (EndpointNotFoundException)
        {
            _logger.LogError("CustomerManagement Endpoint '{uri}' not found", _httpClient.BaseAddress);

            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, $"CustomerManagement Endpoint '{_httpClient.BaseAddress}' not found", 9001);
        }
        catch (Exception ex) when (ex is InvalidOperationException || (ex is TApiException api && GetApiExceptionStatusCode(api) == 500))
        {
            _logger.LogError(ex, ex.Message);

            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, $"CustomerManagement Endpoint '{_httpClient.BaseAddress}' unavailable", 9000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    protected abstract int GetApiExceptionStatusCode(TApiException ex);

    protected abstract object GetApiExceptionDetail(TApiException ex);
}