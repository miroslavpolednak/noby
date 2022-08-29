namespace DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;

public interface ICustomerProfileClient
{
    Task<bool> ValidateProfile(long customerId, string profileCode, string traceId, CancellationToken cancellationToken);
}