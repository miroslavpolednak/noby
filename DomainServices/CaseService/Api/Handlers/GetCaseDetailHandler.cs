using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Handlers;

internal class GetCaseDetailHandler
    : IRequestHandler<Dto.GetCaseDetailMediatrRequest, Case>
{
    /// <summary>
    /// Vraci detail Case-u
    /// </summary>
    public async Task<Case> Handle(Dto.GetCaseDetailMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCaseDetailHandler), request.CaseId);

        // vytahnout Case z DB
        var caseInstance = await _repository.GetCaseDetail(request.CaseId, cancellation);

        return caseInstance;
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<GetCaseDetailHandler> _logger;

    public GetCaseDetailHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<GetCaseDetailHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
