using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;
using contracts = DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.Customer.Search;

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

        var dsRequest = request.SearchData?.ToDomainServiceRequest() ?? new contracts.SearchCustomersRequest();
        _logger.LogSerializedObject(nameof(dsRequest), dsRequest);

        // zavolat BE sluzbu - domluva je takova, ze strankovani BE sluzba zatim nebude podporovat
        var result = ServiceCallResult.ResolveAndThrowIfError<contracts.SearchCustomersResponse>(await _customerService.SearchCustomers(dsRequest, cancellationToken));
        _logger.FoundItems(result.Customers.Count, nameof(contracts.SearchCustomersItem));

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