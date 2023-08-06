using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.CustomerService.ExternalService.Address.V2.Contracts;

namespace DomainServices.CustomerService.ExternalService.Address.V2;

public interface ICustomerAddressServiceClient : IExternalServiceClient
{
    const string Version = "V2";

    Task<string> FormatAddress(ComponentAddressPoint componentAddress, CancellationToken cancellationToken);
}