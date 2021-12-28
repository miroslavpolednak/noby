namespace DomainServices.CaseService.Api.Handlers;

internal class LinkOwnerToCaseHandler
    : IRequestHandler<Dto.LinkOwnerToCaseMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.LinkOwnerToCaseMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Link owner #{userId} to case #{caseId}", request.UserId, request.CaseId);

        var caseInstance = await _repository.GetCaseDetail(request.CaseId);
        //TODO nejaka validace na case?

        // update majitele v databazi
        await _repository.LinkOwnerToCase(request.CaseId, request.UserId);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<LinkOwnerToCaseHandler> _logger;

    public LinkOwnerToCaseHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<LinkOwnerToCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}