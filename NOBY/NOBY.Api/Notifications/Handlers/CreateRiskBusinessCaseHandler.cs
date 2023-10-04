using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Notifications.Handlers;

/// <summary>
/// Zalozi RiskBusinessCase pro dany Sales Arrangement
/// </summary>
internal sealed class CreateRiskBusinessCaseHandler
    : INotificationHandler<MainCustomerUpdatedNotification>
{
    public async Task Handle(MainCustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        //SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(notification.SalesArrangementId, cancellationToken);
        if (string.IsNullOrEmpty(saInstance.RiskBusinessCaseId)) 
        {
            try
            {
                await _createRiskBusiness.Create(notification.CaseId, notification.SalesArrangementId, notification.CustomerOnSAId, notification.CustomerIdentifiers, cancellationToken);
            }
            catch (BaseCisException ex)
            {
                _logger.LogInformation($"CreateRBC failed: {ex.ExceptionCode}; {ex.Message}");
            }
        }
        else // RBCID je jiz zalozene, ukonci flow
        {
            _logger.LogInformation($"SalesArrangement #{notification.SalesArrangementId} already contains RiskBusinessCaseId");
        }
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ILogger<CreateRiskBusinessCaseHandler> _logger;
    private readonly NOBY.Services.CreateRiskBusinessCase.CreateRiskBusinessCaseService _createRiskBusiness;

    public CreateRiskBusinessCaseHandler(NOBY.Services.CreateRiskBusinessCase.CreateRiskBusinessCaseService createRiskBusiness, ISalesArrangementServiceClient salesArrangementService, ILogger<CreateRiskBusinessCaseHandler> logger)
    {
        _salesArrangementService = salesArrangementService;
        _createRiskBusiness = createRiskBusiness;
        _logger = logger;
    }
}
