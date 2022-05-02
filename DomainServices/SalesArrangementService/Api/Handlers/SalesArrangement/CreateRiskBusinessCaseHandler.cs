using DomainServices.SalesArrangementService.Contracts;
using ExternalServices.Rip.V1;
using ExternalServices.Rip.V1.RipWrapper;

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

        // TODO: ziskat RBCID
        /*
        // System = "NOBY" ... přidat do konfigurace a do externí služby posílat jen SalesArrangementId ???
        var ripCreateRequest = new CreateRequest {
            LoanApplicationIdMp = new SystemId { Id = saInstance.SalesArrangementId.ToString(System.Globalization.CultureInfo.InvariantCulture), System = "NOBY" },
            ResourceProcessIdMp = "NOBY",
            ItChannel = "NOBY"
        };
        */

        var riskBusinessCaseId = saInstance.SalesArrangementId.ToString(System.Globalization.CultureInfo.InvariantCulture); //TODO: ServiceCallResult.ResolveAndThrowIfError<string>(await _ripClient.CreateRiskBusinesCase(ripCreateRequest));

        // ulozit ho do DB
        saInstance.RiskBusinessCaseId = riskBusinessCaseId;
        await _repository.SaveChangesAsync(cancellation);

        return new CreateRiskBusinessCaseResponse
        {
            RequestId = "zzzz",
            RiskBusinessCaseId = riskBusinessCaseId
        };
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<CreateRiskBusinessCaseHandler> _logger;
    private readonly IRipClient _ripClient;

    public CreateRiskBusinessCaseHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<CreateRiskBusinessCaseHandler> logger,
        IRipClient ripClient)
    {
        _repository = repository;
        _logger = logger;
        _ripClient = ripClient;
    }
}
