using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Case = DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Abstraction;

namespace FOMS.Api.Notifications.Handlers;

internal class UpdateCustomerOnCaseHandler
    : INotificationHandler<MainCustomerUpdatedNotification>
{
    public async Task Handle(MainCustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateCustomerOnCaseHandler), notification.SalesArrangementId);

        // detail customera
        var customerInstance = ServiceCallResult.Resolve<_SA.CustomerOnSA>(await _customerService.GetCustomer(notification.CustomerOnSAId, cancellationToken));

        // update case detailu
        await _caseService.UpdateCaseCustomer(notification.CaseId, new _Case.CustomerData
        {
            DateOfBirthNaturalPerson = customerInstance.DateOfBirthNaturalPerson,
            FirstNameNaturalPerson = customerInstance.FirstNameNaturalPerson,
            Name = customerInstance.Name,
            Identity = customerInstance.CustomerIdentifiers?.First()
        }, cancellationToken);
    }

    private readonly ICaseServiceAbstraction _caseService;
    private readonly ICustomerOnSAServiceAbstraction _customerService;
    private readonly ILogger<UpdateCustomerOnCaseHandler> _logger;

    public UpdateCustomerOnCaseHandler(
        ICustomerOnSAServiceAbstraction customerService,
        ICaseServiceAbstraction caseService,
        ILogger<UpdateCustomerOnCaseHandler> logger)
    {
        _customerService = customerService;
        _caseService = caseService;
        _logger = logger;
    }
}
