using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomerChangeData;

[TransientService, SelfService]
public class CustomerChangeDataLoader
{
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerChangeDataMerger _customerChangeDataMerger;

    public CustomerChangeDataLoader(ICustomerOnSAServiceClient customerOnSAService, ICustomerChangeDataMerger customerChangeDataMerger)
    {
        _customerOnSAService = customerOnSAService;
        _customerChangeDataMerger = customerChangeDataMerger;
    }

    public async Task LoadChangedCustomerData(CustomerDetailResponse customer, int salesArrangementId, CancellationToken cancellationToken)
    {
        var kbIdentity = customer.Identities.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

        if (kbIdentity is null)
            return;

        var customerOnSaList = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);

        var customerOnSa = customerOnSaList.FirstOrDefault(c => c.CustomerIdentifiers.Any(i => i.IdentityId == kbIdentity.IdentityId && i.IdentityScheme == Identity.Types.IdentitySchemes.Kb));

        if (customerOnSa is null)
            return;

        _customerChangeDataMerger.Merge(customer, customerOnSa);
    }

    public async Task LoadChangedCustomerData(IEnumerable<CustomerDetailResponse> customers, int salesArrangementId, CancellationToken cancellationToken)
    {
        var customerOnSaList = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);

        var customersWithIdentity = customers.Select(c => new { Customer = c, KbIdentity = c.Identities.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb) })
                                             .Where(c => c.KbIdentity is not null);

        var customersOnSaWithIdentity = customerOnSaList.Select(c => new { CustomerOnSa = c, KbIdentity = c.CustomerIdentifiers.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb) })
                                                        .Where(c => c.KbIdentity is not null);

        var customersPair = customersWithIdentity.Join(customersOnSaWithIdentity, x => x.KbIdentity, y => y.KbIdentity, (customer, customerOnSa) => new { customer.Customer, customerOnSa.CustomerOnSa });

        foreach (var pair in customersPair)
        {
            _customerChangeDataMerger.Merge(pair.Customer, pair.CustomerOnSa);
        }
    }
}