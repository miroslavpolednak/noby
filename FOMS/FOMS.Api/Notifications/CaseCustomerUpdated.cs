using CIS.Core.Results;

namespace FOMS.Api.Notifications;

internal sealed class CaseCustomerUpdated
    : INotificationHandler<Requests.CaseCustomerUpdatedRequest>
{
    public async Task Handle(Requests.CaseCustomerUpdatedRequest notification, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Updating case customer {data}", notification);

        var model = new DomainServices.CaseService.Contracts.CustomerData
        {
            FirstNameNaturalPerson = notification.FirstNameNaturalPerson,
            Name = notification.Name,
            DateOfBirthNaturalPerson = notification.DateOfBirthNaturalPerson
        };
        if (notification.Customer is not null)
            model.Identity = notification.Customer;

        ServiceCallResult.Resolve(await _caseService.UpdateCaseCustomer(notification.CaseId, model));

        _logger.LogDebug("Case customer updated");
    }

    private ILogger<CaseCustomerUpdated> _logger;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public CaseCustomerUpdated(DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService, ILogger<CaseCustomerUpdated> logger)
    {
        _logger = logger;
        _caseService = caseService;
    }
}
