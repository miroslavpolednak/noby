using System.ComponentModel;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using DomainServices.CustomerService.Api.Services.KonsDb;

namespace DomainServices.CustomerService.Api.Endpoints.SearchCustomers;

internal class SearchCustomersHandler : IRequestHandler<SearchCustomersRequest, SearchCustomersResponse>
{
    private readonly CustomerManagementSearchProvider _cmSearchProvider;
    private readonly KonsDbSearchProvider _konsDbSearchProvider;

    public SearchCustomersHandler(CustomerManagementSearchProvider cmSearchProvider, KonsDbSearchProvider konsDbSearchProvider)
    {
        _cmSearchProvider = cmSearchProvider;
        _konsDbSearchProvider = konsDbSearchProvider;
    }

    public async Task<SearchCustomersResponse> Handle(SearchCustomersRequest request, CancellationToken cancellationToken)
    {
        var response = new SearchCustomersResponse();

        var customers = request.Mandant switch
        {
            Mandants.Kb => await _cmSearchProvider.Search(request, cancellationToken),
            Mandants.Mp => await _konsDbSearchProvider.Search(request, cancellationToken),
            _ => throw new InvalidEnumArgumentException()
        };

        response.Customers.AddRange(customers);

        return response;
    }
}