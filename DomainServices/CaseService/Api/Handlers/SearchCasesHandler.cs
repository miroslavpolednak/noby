using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Handlers;

internal class SearchCasesHandler
    : IRequestHandler<Dto.SearchCasesMediatrRequest, SearchCasesResponse>
{
    /// <summary>
    /// Seznam Case s moznosti strankovani. Vetsinou pro daneho uzivatele.
    /// </summary>
    public async Task<SearchCasesResponse> Handle(Dto.SearchCasesMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogDebug("Search cases {RecordOffset}/{PageSize} with custom sorting {sorting}", request.Pagination.RecordOffset, request.Pagination.PageSize, request.Pagination.Sorting.Any());
        if (request.Pagination.Sorting.Any())
            _logger.LogDebug("Sorting {field}/{desc}", request.Pagination.Sorting.First().Field, request.Pagination.Sorting.First().Descending);

        return await _repository.GetCaseList(request.Pagination, request.UserId, request.State, request.SearchTerm);
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;
    
    public SearchCasesHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
