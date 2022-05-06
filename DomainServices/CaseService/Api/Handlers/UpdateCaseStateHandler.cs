namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateCaseStateHandler
    : IRequestHandler<Dto.UpdateCaseStateMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseStateMediatrRequest request, CancellationToken cancellation)
    {
        // zjistit zda existuje case
        await _repository.EnsureExistingCase(request.CaseId, cancellation);

        // overit ze case state existuje
        if (!(await _codebookService.CaseStates(cancellation)).Any(t => t.Id == request.State))
            throw new CisNotFoundException(13011, nameof(request.State), request.State);

        // update v DB
        await _repository.UpdateCaseState(request.CaseId, request.State, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<UpdateCaseStateHandler> _logger;

    public UpdateCaseStateHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.CaseServiceRepository repository,
        ILogger<UpdateCaseStateHandler> logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}

