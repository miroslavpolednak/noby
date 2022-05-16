using DomainServices.CaseService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.SendToCmp;

internal class SendToCmpHandler
    : AsyncRequestHandler<SendToCmpRequest>
{
    protected override async Task Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        // odeslat do SB
        await _salesArrangementService.SendToCmp(request.SalesArrangementId, cancellationToken);

        // update case state
        await _caseService.UpdateCaseState(saInstance.CaseId, 2, cancellationToken);
    }

    private readonly ICaseServiceAbstraction _caseService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<SendToCmpHandler> _logger;

    public SendToCmpHandler(
        ICaseServiceAbstraction caseService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        ILogger<SendToCmpHandler> logger)
    {
        _caseService = caseService;
        _logger = logger;
        _salesArrangementService = salesArrangementService;
    }
}
