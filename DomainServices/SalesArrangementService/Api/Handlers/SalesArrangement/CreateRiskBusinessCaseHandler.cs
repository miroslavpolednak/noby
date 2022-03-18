using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class CreateRiskBusinessCaseHandler
    : IRequestHandler<Dto.CreateRiskBusinessCaseMediatrRequest, CreateRiskBusinessCaseResponse>
{
    public async Task<CreateRiskBusinessCaseResponse> Handle(Dto.CreateRiskBusinessCaseMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateRiskBusinessCaseHandler), request.SalesArrangementId);

        // instance SA
        var saInstance = await _repository.GetSalesArrangementEntity(request.SalesArrangementId, cancellation);

        // kontroly
        if (!saInstance.OfferId.HasValue)
            throw new CisNotFoundException(16000, $"Sales Arrangement #{request.SalesArrangementId} is not linked to Offer");

        if (!string.IsNullOrEmpty(saInstance.RiskBusinessCaseId))
            throw CIS.Infrastructure.gRPC.GrpcExceptionHelpers.CreateRpcException(Grpc.Core.StatusCode.AlreadyExists, $"Sales Arrangement #{request.SalesArrangementId} already contains RiskBusinessCaseId {saInstance.RiskBusinessCaseId}", 16000);

        // ziskat RBCID

        // ulozit ho do DB
        saInstance.RiskBusinessCaseId = "xxxx";
        await _repository.SaveChangesAsync(cancellation);

        return new CreateRiskBusinessCaseResponse
        {
            RequestId = "zzzz",
            RiskBusinessCaseId = "xxxx"
        };
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<CreateRiskBusinessCaseHandler> _logger;

    public CreateRiskBusinessCaseHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<CreateRiskBusinessCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
