using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;
using DomainServices.CustomerService.Api.Services.CustomerSource.CustomerManagement;
using System.Diagnostics;
using CIS.Infrastructure.Attributes;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api.Services.CustomerSource;

[ScopedService, SelfService]
public class CustomerSourceManager
{
    private readonly ICustomerManagementClient _customerManagement;
    private readonly ICodebookServiceAbstraction _codebookService;

    public CustomerSourceManager(ICustomerManagementClient customerManagement, ICodebookServiceAbstraction codebookService)
    {
        _customerManagement = customerManagement;
        _codebookService = codebookService;
    }

    public async Task<CustomerDetailResponse> GetDetail(Identity identity, CancellationToken cancellationToken)
    {
        CustomerDetailResponse detail = null!;

        if (identity.IdentityScheme == Identity.Types.IdentitySchemes.Kb)
        {
            var customer = await _customerManagement.GetDetail(identity.IdentityId, Activity.Current?.TraceId.ToHexString() ?? "", CancellationToken.None);

            var parser = await KBCustomerDetailParser.CreateInstance(_codebookService, cancellationToken);

            detail = parser.Parse(customer);
        }

        return detail;
    }

    public async Task<IEnumerable<CustomerListItem>> GetList(IEnumerable<Identity> identities, CancellationToken cancellationToken)
    {
        var identitiesLookup = identities.ToLookup(x => x.IdentityScheme, y => y.IdentityId);

        var customers = await GetKbCustomerList(identitiesLookup[Identity.Types.IdentitySchemes.Kb], cancellationToken);

        return customers;
    }

    public async Task<IEnumerable<SearchCustomersItem>> Search(SearchCustomersRequest request, CancellationToken cancellationToken)
    {
        if (request.Mandant == Mandants.Kb)
        {
            var parser = await KBSearchCustomersParser.CreateInstance(_codebookService, cancellationToken);

            var foundCustomers = await _customerManagement.Search(parser.ParseRequest(request), Activity.Current?.TraceId.ToHexString() ?? "", cancellationToken);

            return foundCustomers.Where(c => c.Party is NaturalPersonSearchResult).Select(parser.ParseResult);
        }


        return Enumerable.Empty<SearchCustomersItem>();
    }

    private async Task<IEnumerable<CustomerListItem>> GetKbCustomerList(IEnumerable<long> customerIds, CancellationToken cancellationToken)
    {
        if (!customerIds.Any())
            return Enumerable.Empty<CustomerListItem>();

        var customers = await _customerManagement.GetList(customerIds, Activity.Current?.TraceId.ToHexString() ?? "", cancellationToken);

        var customerParser = await KBCustomerListItemParser.CreateInstance(_codebookService, cancellationToken);

        return customers.Select(customerParser.Parse);
    }
}