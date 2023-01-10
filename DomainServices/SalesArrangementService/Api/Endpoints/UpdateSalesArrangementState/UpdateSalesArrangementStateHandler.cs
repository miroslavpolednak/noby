namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangementState;

internal sealed class UpdateSalesArrangementStateHandler
    : IRequestHandler<Contracts.UpdateSalesArrangementStateRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Contracts.UpdateSalesArrangementStateRequest request, CancellationToken cancellation)
    {
        // kontrola existence noveho stavu
        _ = (await _codebookService.SalesArrangementStates(cancellation)).FirstOrDefault(t => t.Id == request.State)
            ?? throw new CisNotFoundException(18006, $"SalesArrangementState #{request.State} does not exist.");

        // kontrola existence SA
        var saEntity = await _repository.GetSalesArrangement(request.SalesArrangementId, cancellation);

        // kontrola aktualniho stavu vuci novemu stavu
        if (saEntity.State == request.State)
            throw new CisValidationException(18007, $"SalesArrangement {request.SalesArrangementId} is already in state {request.State}");

        // update stavu SA
        await _repository.UpdateSalesArrangementState(request.SalesArrangementId, request.State, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly Database.SalesArrangementServiceRepository _repository;

    public UpdateSalesArrangementStateHandler(
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Database.SalesArrangementServiceRepository repository)
    {
        _codebookService = codebookService;
        _repository = repository;
    }
}
