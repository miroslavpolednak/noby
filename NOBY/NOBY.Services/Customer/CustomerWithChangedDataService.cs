using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Clients;
using NOBY.Dto.Customer;

namespace NOBY.Services.Customer;

[ScopedService, SelfService]
public class CustomerWithChangedDataService
{
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICustomerChangeDataMerger _customerChangeDataMerger;

    public CustomerWithChangedDataService(ICustomerOnSAServiceClient customerOnSAService,
                                          ICustomerServiceClient customerService,
                                          ICustomerChangeDataMerger customerChangeDataMerger)
    {
        _customerOnSAService = customerOnSAService;
        _customerService = customerService;
        _customerChangeDataMerger = customerChangeDataMerger;
    }

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

    public async Task<TCustomer> GetCustomerResponseDto<TCustomer>(int customerOnSaId, CancellationToken cancellationToken = default) where TCustomer : BaseCustomerDetail
    {
        var info = await GetCustomerInfo(customerOnSaId, cancellationToken);

        return CustomerMapper.MapCustomerToResponseDto<TCustomer>(info.CustomerDetail, info.CustomerOnSA);
    }
}