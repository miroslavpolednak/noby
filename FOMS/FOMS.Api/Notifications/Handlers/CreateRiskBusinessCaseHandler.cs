using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Notifications.Handlers;

/// <summary>
/// Zalozi RiskBusinessCase pro dany Sales Arrangement
/// </summary>
internal class CreateRiskBusinessCaseHandler
    : INotificationHandler<CustomerFullyIdentifiedNotification>
{
    public async Task Handle(CustomerFullyIdentifiedNotification notification, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateRiskBusinessCaseHandler), notification.SalesArrangementId);

        await _salesArrangementService.CreateRiskBusinessCase(notification.SalesArrangementId, cancellationToken);
    }

    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<CreateRiskBusinessCaseHandler> _logger;

    public CreateRiskBusinessCaseHandler(
        ISalesArrangementServiceAbstraction salesArrangementService,
        ILogger<CreateRiskBusinessCaseHandler> logger)
    {
        _salesArrangementService = salesArrangementService;
        _logger = logger;
    }
}
