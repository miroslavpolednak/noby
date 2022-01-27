using CIS.Core.Results;
using CIS.Core.Types;

namespace FOMS.Api.Endpoints.Case.Handlers;

internal class SearchHandler
    : IRequestHandler<Dto.SearchRequest, Dto.SearchResponse>
{
    public async Task<Dto.SearchResponse> Handle(Dto.SearchRequest request, CancellationToken cancellationToken)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination)
            .EnsureAndTranslateSortFields(sortingMapper);

        _logger.LogDebug("Search for user {userId} with {pagination}", _userAccessor.User.Id, paginable);

        // zavolat BE sluzbu
        var result = ServiceCallResult.Resolve<DomainServices.CaseService.Contracts.SearchCasesResponse>(await _caseService.SearchCases(paginable, _userAccessor.User.Id, request.State, request.Term, cancellationToken));
        _logger.LogDebug("Found {records} records", result.Pagination.RecordsTotalSize);

        // transform
        return new Dto.SearchResponse
        {
            Rows = await _converter.FromContracts(result.CaseInstances),
            Pagination = new CIS.Infrastructure.WebApi.Types.PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, result.Pagination.RecordsTotalSize)
        };
    }

    private static List<Paginable.MapperField> sortingMapper = new()
    {
        new ("stateUpdated", "StateUpdatedOn"),
        new ("customerName", "Name")
    };

    private readonly ILogger<SearchHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly CaseModelConverter _converter;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public SearchHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
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
