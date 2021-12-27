using CIS.Core.Results;

namespace FOMS.Api.Notifications;

internal sealed class CaseCustomerUpdated
    : INotificationHandler<Requests.CaseCustomerUpdatedRequest>
{
    public async Task Handle(Requests.CaseCustomerUpdatedRequest notification, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Updating case customer {data}", notification);

        resolveResult(await _caseService.UpdateCaseCustomer(new DomainServices.CaseService.Contracts.UpdateCaseCustomerRequest
        {
            CaseId = notification.CaseId,
            FirstNameNaturalPerson = notification.FirstNameNaturalPerson,
            Name = notification.Name,
            DateOfBirthNaturalPerson = notification.DateOfBirthNaturalPerson,
            Customer = notification.Customer
        }));

        _logger.LogDebug("Case customer updated");
    }

    private bool resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult => true,
            _ => throw new NotImplementedException()
        };

    private ILogger<CaseCustomerUpdated> _logger;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public CaseCustomerUpdated(DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService, ILogger<CaseCustomerUpdated> logger)
    {
        _logger = logger;
        _caseService = caseService;
    }
}
