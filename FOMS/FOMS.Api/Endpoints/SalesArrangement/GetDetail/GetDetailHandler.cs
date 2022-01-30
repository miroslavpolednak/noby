using CIS.Core.Results;

namespace FOMS.Api.Endpoints.SalesArrangement.Handlers;

internal class GetDetailHandler
    : IRequestHandler<Dto.GetDetailRequest, DomainServices.SalesArrangementService.Contracts.SalesArrangement>
{
    public async Task<DomainServices.SalesArrangementService.Contracts.SalesArrangement> Handle(Dto.GetDetailRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Get #{salesArrangementId}", request.SalesArrangementId);

        return resolveResult(await _saService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));
    }

    private DomainServices.SalesArrangementService.Contracts.SalesArrangement resolveResult(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<DomainServices.SalesArrangementService.Contracts.SalesArrangement> r => r.Model,
           _ => throw new NotImplementedException()
       };

    private readonly ILogger<GetDetailHandler> _logger;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _saService;

    public GetDetailHandler(
        ILogger<GetDetailHandler> logger,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction saService)
    {
        _logger = logger;
        _saService = saService;
    }
}
