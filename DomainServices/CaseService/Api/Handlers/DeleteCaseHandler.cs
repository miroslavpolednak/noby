namespace DomainServices.CaseService.Api.Handlers;

internal class DeleteCaseHandler
    : IRequestHandler<Dto.DeleteCaseMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.DeleteCaseMediatrRequest request, CancellationToken cancellation)
    {
        // ulozit do DB
        await _repository.DeleteCase(request.CaseId, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<DeleteCaseHandler> _logger;

    public DeleteCaseHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<DeleteCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
