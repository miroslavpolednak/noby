using System.ServiceModel;
using CIS.Core.Exceptions;
using DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;
using CIS.Infrastructure.gRPC;
using Grpc.Core;

namespace DomainServices.CustomerService.Api.Clients.CustomerProfile;

public abstract class BaseClient
{
    private readonly HttpClient _httpClient;
    protected readonly ILogger _logger;

    protected BaseClient(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    protected async Task<TResult> CallEndpoint<TResult>(Func<CustomerProfileWrapper, Task<TResult>> func)
    {
        try
        {
            return await func(CreateClientWrapper());
        }
        catch (ApiException<Error> ex) when (ex.StatusCode == 404)
        {
            _logger.LogError("Customer was not found: {detail}", ex.Result.Detail);

            throw new CisNotFoundException(99999, "Customer not found"); //TODO: ErrorCode
        }
        catch (ApiException<Error> ex) when (ex.StatusCode != 500)
        {
            _logger.LogError("Incorrect inputs to CustomerManagement: {detail}", ex.Result.Detail);

            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.InvalidArgument, $"Incorrect inputs to CustomerManagement: {ex.Result.Detail}", 99999); //TODO: ErrorCode
        }
        catch (EndpointNotFoundException)
        {
            _logger.LogError("CustomerManagement Endpoint '{uri}' not found", _httpClient.BaseAddress);

            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, $"CustomerManagement Endpoint '{_httpClient.BaseAddress}' not found", 9001);
        }
        catch (Exception ex) when (ex is InvalidOperationException or ApiException { StatusCode: 500 })
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

    private CustomerProfileWrapper CreateClientWrapper() => new(_httpClient);
}