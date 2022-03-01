using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;
using contracts = DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.Customer.Search;

internal class SearchHandler
    : IRequestHandler<SearchRequest, SearchResponse>
{
    public async Task<SearchResponse> Handle(SearchRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStarted(nameof(SearchHandler));
        
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination)
            .EnsureAndTranslateSortFields(sortingMapper)
            .SetDefaultSort("LastName", true);

        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var result = ServiceCallResult.Resolve<contracts.SearchCustomersResponse>(await _customerService.SearchCustomers(request.SearchData?.ToDomainServiceRequest() ?? new contracts.SearchCustomersRequest(), cancellationToken));
        _logger.FoundItems(result.Customers.Count, nameof(contracts.SearchCustomerResult));

        // transform
        return new SearchResponse
        {
            Rows = result.Customers.ToApiResponse(),
            Pagination = new PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, result.Customers.Count)
        };
    }

    private static List<Paginable.MapperField> sortingMapper = new()
    {
        new ("lastName", "LastName")
    };

    private readonly ILogger<SearchHandler> _logger;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public SearchHandler(
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        ILogger<SearchHandler> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }
}