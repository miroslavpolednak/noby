namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class UpdateSalesArrangementStateHandler
    : IRequestHandler<Dto.UpdateSalesArrangementStateMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementStateMediatrRequest request, CancellationToken cancellation)
    {
        // kontrola existence noveho stavu
        _ = (await _codebookService.SalesArrangementStates(cancellation)).FirstOrDefault(t => t.Id == request.State)
            ?? throw new CisNotFoundException(18006, $"SalesArrangementState #{request.State} does not exist.");

        // kontrola existence SA
        var saEntity = await _repository.GetSalesArrangement(request.SalesArrangementId, cancellation);

        // kontrola aktualniho stavu vuci novemu stavu
        if (saEntity.State == request.State)
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.InvalidArgument, $"SalesArrangement {request.SalesArrangementId} is already in state {request.State}", 18007);

        // update stavu SA
        await _repository.UpdateSalesArrangementState(request.SalesArrangementId, request.State, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<UpdateSalesArrangementStateHandler> _logger;

    public UpdateSalesArrangementStateHandler(
        CodebookService.Clients.ICodebookServiceClients codebookService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<UpdateSalesArrangementStateHandler> logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}
