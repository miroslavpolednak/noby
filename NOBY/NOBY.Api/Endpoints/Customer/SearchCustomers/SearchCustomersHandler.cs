using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;
using DomainServices.CustomerService.Clients;
using __Customer = DomainServices.CustomerService.Contracts;

namespace NOBY.Api.Endpoints.Customer.SearchCustomers;

internal sealed class SearchCustomersHandler
    : IRequestHandler<SearchCustomersRequest, SearchCustomersResponse>
{
    public async Task<SearchCustomersResponse> Handle(SearchCustomersRequest request, CancellationToken cancellationToken)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination)
            .EnsureAndTranslateSortFields(sortingMapper)
            .SetDefaultSort("LastName", true);

        var dsRequest = request.SearchData?.ToDomainServiceRequest() ?? new __Customer.SearchCustomersRequest();
        
        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var searchResult = await _customerService.SearchCustomers(dsRequest, cancellationToken);

        if (searchResult.Customers.Any())
        {
            return new SearchCustomersResponse
            {
                Rows = searchResult.Customers.ToApiResponse(),
                Pagination = new PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, searchResult.Customers.Count)
            };
        }

        return new SearchCustomersResponse()
        {
            Pagination = new PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, 0)
        };
    }

    private static List<Paginable.MapperField> sortingMapper = new()
    {
        new ("lastName", "LastName")
    };

    private readonly ICustomerServiceClient _customerService;

    public SearchCustomersHandler(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}