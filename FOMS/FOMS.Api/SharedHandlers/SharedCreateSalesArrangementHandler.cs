using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.SharedHandlers;

internal sealed class SharedCreateSalesArrangementHandler
    : IRequestHandler<Requests.SharedCreateSalesArrangementRequest, int>
{
    public async Task<int> Handle(Requests.SharedCreateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Attempt to create sales arrangement {data}", request);
        int saId = ServiceCallResult.Resolve<int>(await _salesArrangementService.CreateSalesArrangement(request.CaseId, request.ProductInstanceType, request.OfferInstanceId, cancellationToken));
        _logger.LogDebug("Sales arrangement #{saId} created", saId);

        return saId;
    }

    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private ILogger<SharedCreateSalesArrangementHandler> _logger;

    public SharedCreateSalesArrangementHandler(
        ILogger<SharedCreateSalesArrangementHandler> logger,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _logger = logger;
        _salesArrangementService = salesArrangementService;
    }
}
