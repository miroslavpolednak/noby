using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;
using DomainServices.CustomerService.Clients;
using __Customer = DomainServices.CustomerService.Contracts;

namespace NOBY.Api.Endpoints.Customer.Search;

internal class SearchHandler
    : IRequestHandler<SearchRequest, SearchResponse>
{
    public async Task<SearchResponse> Handle(SearchRequest request, CancellationToken cancellationToken)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination)
            .EnsureAndTranslateSortFields(sortingMapper)
            .SetDefaultSort("LastName", true);

        var dsRequest = request.SearchData?.ToDomainServiceRequest() ?? new __Customer.SearchCustomersRequest();
        
        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var rawResult = await _customerService.SearchCustomers(dsRequest, cancellationToken);
        if (rawResult is EmptyServiceCallResult)
        {
            return new SearchResponse
            {
                Pagination = new PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, 0)
            };
        }
        else
        {
            var result = ServiceCallResult.ResolveAndThrowIfError<__Customer.SearchCustomersResponse>(rawResult);
            
            // transform
            return new SearchResponse
            {
                Rows = result.Customers.ToApiResponse(),
                Pagination = new PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, result.Customers.Count)
            };
        }
    }

    private static List<Paginable.MapperField> sortingMapper = new()
    {
        new ("lastName", "LastName")
    };

    private readonly ICustomerServiceClient _customerService;

    public SearchHandler(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}