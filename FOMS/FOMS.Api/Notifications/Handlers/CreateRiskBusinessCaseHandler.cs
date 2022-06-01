using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Notifications.Handlers;

/// <summary>
/// Zalozi RiskBusinessCase pro dany Sales Arrangement
/// </summary>
internal class CreateRiskBusinessCaseHandler
    : INotificationHandler<MainCustomerUpdatedNotification>
{
    public async Task Handle(MainCustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        if (!notification.NewMpCustomerId.HasValue) return;

        await _salesArrangementService.CreateRiskBusinessCase(notification.SalesArrangementId, cancellationToken);
    }

    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;

    public CreateRiskBusinessCaseHandler(
        ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}
