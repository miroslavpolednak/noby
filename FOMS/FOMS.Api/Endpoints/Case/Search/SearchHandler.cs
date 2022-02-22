using CIS.Core.Security;
using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;
using DSContracts = DomainServices.CaseService.Contracts;

namespace FOMS.Api.Endpoints.Case.Search;

internal class SearchHandler
    : IRequestHandler<SearchRequest, SearchResponse>
{
    public async Task<SearchResponse> Handle(SearchRequest request, CancellationToken cancellationToken)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination)
            .EnsureAndTranslateSortFields(sortingMapper);

        _logger.LogDebug("Search for user {userId} with {pagination}", _userAccessor.User.Id, paginable);

        // zavolat BE sluzbu
        var result = ServiceCallResult.Resolve<DSContracts.SearchCasesResponse>(await _caseService.SearchCases(paginable, _userAccessor.User.Id, request.State, request.Term, cancellationToken));
        _logger.LogDebug("Found {records} records", result.Pagination.RecordsTotalSize);

        // transform
        return new SearchResponse
        {
            Rows = await _converter.FromContracts(result.Cases),
            Pagination = new PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, result.Pagination.RecordsTotalSize)
        };
    }

    private static List<Paginable.MapperField> sortingMapper = new()
    {
        new ("stateUpdated", "StateUpdatedOn"),
        new ("customerName", "Name")
    };

    private readonly ILogger<SearchHandler> _logger;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly CaseModelConverter _converter;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public SearchHandler(
        ICurrentUserAccessor userAccessor,
        ILogger<SearchHandler> logger,
        CaseModelConverter converter,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _converter = converter;
        _userAccessor = userAccessor;
        _logger = logger;
        _caseService = caseService;
    }
}
