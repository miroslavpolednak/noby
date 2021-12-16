namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateCaseStateHandler
    : IRequestHandler<Dto.UpdateCaseStateMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseStateMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Update case state for #{caseId} to {state}", request.CaseId, request.State);

        var caseInstance = await _repository.GetCaseDetail(request.CaseId);
        //TODO nejaka validace na case?

        await _repository.UpdateCaseState(request.CaseId, request.State);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;

    public UpdateCaseStateHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}

