using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.SharedHandlers;

internal sealed class SharedCreateSalesArrangementHandler
    : IRequestHandler<Requests.SharedCreateSalesArrangementRequest, int>
{
    public async Task<int> Handle(Requests.SharedCreateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        _logger.SharedCreateSalesArrangementStarted(request);
        
        int salesArrangementId = ServiceCallResult.Resolve<int>(await _salesArrangementService.CreateSalesArrangement(request.CaseId, request.ProductTypeId, request.OfferId, cancellationToken));
        
        _logger.EntityCreated("SalesArrangement", salesArrangementId);

        return salesArrangementId;
    }

    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<SharedCreateSalesArrangementHandler> _logger;

    public SharedCreateSalesArrangementHandler(
        ILogger<SharedCreateSalesArrangementHandler> logger,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _logger = logger;
        _salesArrangementService = salesArrangementService;
    }
}
