using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Handlers.CaseService;

internal class GetCaseListHandler
    : IRequestHandler<Dto.CaseService.GetCaseListMediatrRequest, GetCaseListResponse>
{
    public async Task<GetCaseListResponse> Handle(Dto.CaseService.GetCaseListMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get list for #{id} in {state}", request.PartyId, request.State);

        return await _repository.GetCaseList(request.PartyId, request.State, request.Pagination);
    }

    private readonly Repositories.NobyDbRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;
    
    public GetCaseListHandler(
        Repositories.NobyDbRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
