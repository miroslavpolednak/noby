using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services;

[TransientService, SelfService]
public class CustomerWithChangesService
{
    private readonly ICustomerServiceClient _customerService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerChangeDataMerger _customerChangeDataMerger;

    public CustomerWithChangesService(ICustomerServiceClient customerService, ICustomerOnSAServiceClient customerOnSAService, ICustomerChangeDataMerger customerChangeDataMerger)
    {
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
        _customerChangeDataMerger = customerChangeDataMerger;
    }

    public async Task<CustomerDetailResponse> GetCustomerDetail(Identity identity, int salesArrangementId, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerDetail(identity, cancellationToken);
        var customerOnSaList = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);

        var customerOnSa = customerOnSaList.FirstOrDefault(c => c.CustomerIdentifiers.Contains(identity));

        if (customerOnSa is not null)
            _customerChangeDataMerger.MergeAll(customer, customerOnSa);

        return customer;
    }

    public async Task<IList<CustomerDetailResponse>> GetCustomerList(IEnumerable<CustomerOnSA> customersOnSa, CancellationToken cancellationToken)
    {
        var customersOnSaWithKbIdentity = GetCustomersOnSaWithIdentity(customersOnSa);

        var response = await _customerService.GetCustomerList(customersOnSaWithKbIdentity.Keys, cancellationToken);

        foreach (var customerDetail in response.Customers)
            MergeCustomerChanges(customerDetail, customersOnSaWithKbIdentity);

        return response.Customers;
    }

    public async Task<IList<CustomerDetailResponse>> GetCustomerList(IEnumerable<Identity> identities, int salesArrangementId, CancellationToken cancellationToken)
    {
        var customers = (await _customerService.GetCustomerList(identities, cancellationToken)).Customers;
        var customersOnSa = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);

        var customersOnSaWithKbIdentity = GetCustomersOnSaWithIdentity(customersOnSa);

        foreach (var customerDetail in customers)
            MergeCustomerChanges(customerDetail, customersOnSaWithKbIdentity);

        return customers;
    }

    private void MergeCustomerChanges(CustomerDetailResponse customerDetail, IReadOnlyDictionary<Identity, CustomerOnSA> customerOnSaWithIdentity)
    {
        var kbIdentity = GetKbIdentity(customerDetail);

        if (kbIdentity is null)
            return;

        var customerOnSa = customerOnSaWithIdentity.GetValueOrDefault(kbIdentity);

        if (customerOnSa is null)
            return;

        _customerChangeDataMerger.MergeAll(customerDetail, customerOnSa);
    }

    private static Dictionary<Identity, CustomerOnSA> GetCustomersOnSaWithIdentity(IEnumerable<CustomerOnSA> customersOnSa) =>
        customersOnSa.Select(c => new
                     {
                         CustomerOnSa = c,
                         KbIdentity = c.CustomerIdentifiers.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb)
                     })
                     .Where(c => c.KbIdentity is not null)
                     .ToDictionary(k => k.KbIdentity!, v => v.CustomerOnSa);

    private static Identity? GetKbIdentity(CustomerDetailResponse customerDetail) => 
        customerDetail.Identities.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
}