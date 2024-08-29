using DomainServices.CustomerService.Clients;
using DomainServices.HouseholdService.Clients.v1;

namespace NOBY.Services.Customer;

[ScopedService, SelfService]
public sealed class CustomerWithChangedDataService(
    ICustomerOnSAServiceClient _customerOnSAService,
    ICustomerServiceClient _customerService,
    DomainServices.HouseholdService.Clients.ICustomerChangeDataMerger _customerChangeDataMerger)
{
    public async Task<CustomerInfo> GetCustomerInfo(int customerOnSaId, CancellationToken cancellationToken = default)
    {
        var customerOnSa = await _customerOnSAService.GetCustomer(customerOnSaId, cancellationToken);

        //KB CM identity must exist
        var kbIdentity = customerOnSa.CustomerIdentifiers.GetKbIdentityOrDefault()
                         ?? throw new CisValidationException("Customer is missing KB identity");

        //KB CM
        var originalCustomer = await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);

        return new CustomerInfo
        {
            CustomerOnSA = customerOnSa,
            CustomerDetail = originalCustomer
        };
    }

    public async Task<CustomerInfoWithChangedData> GetCustomerWithChangedData(int customerOnSaId, CancellationToken cancellationToken = default)
    {
        var customerInfo = await GetCustomerInfo(customerOnSaId, cancellationToken);

        var customerWithChangedData = customerInfo.CustomerDetail.Clone();
        _customerChangeDataMerger.MergeAll(customerWithChangedData, customerInfo.CustomerOnSA);

        return new CustomerInfoWithChangedData
        {
            CustomerOnSA = customerInfo.CustomerOnSA,
            CustomerDetail = customerInfo.CustomerDetail,
            CustomerWithChangedData = customerWithChangedData
        };
    }
}