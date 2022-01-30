namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class UpdateSalesArrangementStateHandler
    : IRequestHandler<Dto.UpdateSalesArrangementStateMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementStateMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogDebug("Update SA #{id} State to {state}", request.SalesArrangementId, request.State);

        // kontrola existence noveho stavu
        var stateInstance = (await _codebookService.SalesArrangementStates()).FirstOrDefault(t => t.Id == request.State)
            ?? throw new CisNotFoundException(16006, $"SalesArrangementState #{request.State} does not exist.");

        // kontrola existence SA
        var saEntity = await _repository.GetSalesArrangement(request.SalesArrangementId, cancellation);

        // kontrola aktualniho stavu vuci novemu stavu
        if (saEntity.State == request.State)
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.InvalidArgument, $"SalesArrangement {request.SalesArrangementId} is already in state {request.State}", 16007);

        // update stavu SA
        await _repository.UpdateSalesArrangementState(request.SalesArrangementId, request.State, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<UpdateSalesArrangementStateHandler> _logger;

    public UpdateSalesArrangementStateHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<UpdateSalesArrangementStateHandler> logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}
