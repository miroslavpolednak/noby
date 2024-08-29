using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services;

[TransientService, SelfService]
public class CustomerWithChangesService(
    ICustomerServiceClient _customerService, 
    ICustomerOnSAServiceClient _customerOnSAService, 
    DomainServices.HouseholdService.Clients.ICustomerChangeDataMerger _customerChangeDataMerger)
{
    public async Task<(CustomerDetailResponse customer, CustomerOnSA? customerOnSA)> GetCustomerDetail(Identity identity, int salesArrangementId, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerDetail(identity, forceKbCustomerLoad: true, cancellationToken);
        var customerOnSaList = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);

        var customerOnSa = customerOnSaList.FirstOrDefault(c => c.CustomerIdentifiers.Contains(identity));

        if (customerOnSa is not null)
        {
            customerOnSa = await _customerOnSAService.GetCustomer(customerOnSa.CustomerOnSAId, cancellationToken);

            _customerChangeDataMerger.MergeAll(customer, customerOnSa);
        }

        return (customer, customerOnSa);
    }

    public async Task<(CustomerDetailResponse customer, CustomerOnSA? customerOnSA)> GetCustomerDetail(int customerOnSaId, CancellationToken cancellationToken)
    {
        await _customerOnSAService.GetCustomer(customerOnSaId, cancellationToken);

        var customerOnSa = await _customerOnSAService.GetCustomer(customerOnSaId, cancellationToken);

        var customerIdentity = customerOnSa.CustomerIdentifiers.GetKbIdentityOrDefault()
                               ?? throw new InvalidOperationException($"CustomerOnSa {customerOnSa} has no identity");

        var customer = await _customerService.GetCustomerDetail(customerIdentity, forceKbCustomerLoad: true, cancellationToken);

        _customerChangeDataMerger.MergeAll(customer, customerOnSa);

        return (customer, customerOnSa);
    }

    public async Task<IList<CustomerDetailResponse>> GetCustomerList(IEnumerable<CustomerOnSA> customersOnSa, CancellationToken cancellationToken)
    {
        var customersOnSaWithKbIdentity = GetCustomersOnSaWithIdentity(customersOnSa);

        var response = await _customerService.GetCustomerList(customersOnSaWithKbIdentity.Keys, cancellationToken);

        foreach (var customerDetail in response.Customers)
        {
            var customerOnSa = GetCustomerOnSA(customerDetail, customersOnSaWithKbIdentity);

            if (customerOnSa is null)
                continue;

            //CustomerOnSa full detail
            customerOnSa = await _customerOnSAService.GetCustomer(customerOnSa.CustomerOnSAId, cancellationToken);

            _customerChangeDataMerger.MergeAll(customerDetail, customerOnSa);
        }

        return response.Customers;
    }

    public async Task<IList<CustomerDetailResponse>> GetCustomerList(IEnumerable<Identity> identities, int salesArrangementId, CancellationToken cancellationToken)
    {
        var customers = (await _customerService.GetCustomerList(identities, cancellationToken)).Customers;
        var customersOnSa = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);

        var customersOnSaWithKbIdentity = GetCustomersOnSaWithIdentity(customersOnSa);

        foreach (var customerDetail in customers)
        {
            var customerOnSa = GetCustomerOnSA(customerDetail, customersOnSaWithKbIdentity);

            if (customerOnSa is null)
                continue;

            //CustomerOnSa full detail
            customerOnSa = await _customerOnSAService.GetCustomer(customerOnSa.CustomerOnSAId, cancellationToken);

            _customerChangeDataMerger.MergeAll(customerDetail, customerOnSa);
        }

        return customers;
    }

    private static Dictionary<Identity, CustomerOnSA> GetCustomersOnSaWithIdentity(IEnumerable<CustomerOnSA> customersOnSa) =>
        customersOnSa.Select(c => new
                     {
                         CustomerOnSa = c,
                         KbIdentity = c.CustomerIdentifiers.GetKbIdentityOrDefault()
                     })
                     .Where(c => c.KbIdentity is not null)
                     .ToDictionary(k => k.KbIdentity!, v => v.CustomerOnSa);

    private static CustomerOnSA? GetCustomerOnSA(CustomerDetailResponse customerDetail, IReadOnlyDictionary<Identity, CustomerOnSA> customersOnSaWithKbIdentity)
    {
        var kbIdentity = customerDetail.Identities.GetKbIdentityOrDefault();

        return kbIdentity is null ? default : customersOnSaWithKbIdentity.GetValueOrDefault(kbIdentity);
    }
}