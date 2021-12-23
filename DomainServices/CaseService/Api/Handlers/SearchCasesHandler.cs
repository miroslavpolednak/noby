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
        _logger.LogInformation("Search cases {request}", request);

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
