using CIS.Core.Results;

namespace FOMS.Api.Notifications;

internal sealed class CaseDataUpdated
    : INotificationHandler<Requests.CaseDataUpdatedRequest>
{
    public async Task Handle(Requests.CaseDataUpdatedRequest notification, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Updating case {data}", notification);

        var model = new DomainServices.CaseService.Contracts.CaseData
        {
            ContractNumber = notification.ContractNumber,
            TargetAmount = notification.TargetAmount,
            ProductInstanceType = 1
        };

        ServiceCallResult.Resolve(await _caseService.UpdateCaseData(notification.CaseId, model, cancellationToken));

        _logger.LogDebug("Case updated");
    }

    private ILogger<CaseDataUpdated> _logger;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public CaseDataUpdated(DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService, ILogger<CaseDataUpdated> logger)
    {
        _logger = logger;
        _caseService = caseService;
    }
}
