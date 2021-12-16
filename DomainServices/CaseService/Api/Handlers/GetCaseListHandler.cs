using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Handlers;

internal class GetCaseListHandler
    : IRequestHandler<Dto.GetCaseListMediatrRequest, GetCaseListResponse>
{
    public async Task<GetCaseListResponse> Handle(Dto.GetCaseListMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Get list for #{id} in {state}", request.UserId, request.State);

        return await _repository.GetCaseList(request.UserId, request.State, request.Pagination);
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;
    
    public GetCaseListHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
