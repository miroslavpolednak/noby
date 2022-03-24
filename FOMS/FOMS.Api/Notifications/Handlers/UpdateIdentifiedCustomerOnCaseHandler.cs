using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using _Customer = DomainServices.CustomerService.Contracts;
using _Case = DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Abstraction;

namespace FOMS.Api.Notifications.Handlers;

internal class UpdateIdentifiedCustomerOnCaseHandler
    : INotificationHandler<CustomerFullyIdentifiedNotification>
{
    public async Task Handle(CustomerFullyIdentifiedNotification notification, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateIdentifiedCustomerOnCaseHandler), notification.SalesArrangementId);

        // detail customera
        var customerInstance = ServiceCallResult.Resolve<_Customer.CustomerResponse>(await _customerService.GetCustomerDetail(new _Customer.CustomerRequest
        {
            Identity = notification.Identity
        }, cancellationToken));

        // update case detailu
        await _caseService.UpdateCaseCustomer(notification.CaseId, new _Case.CustomerData
        {
            DateOfBirthNaturalPerson = customerInstance.NaturalPerson?.DateOfBirth,
            FirstNameNaturalPerson = customerInstance.NaturalPerson?.FirstName,
            Name = customerInstance.NaturalPerson?.LastName,
            Identity = notification.Identity
        }, cancellationToken);
    }

    private readonly ICaseServiceAbstraction _caseService;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly ILogger<UpdateIdentifiedCustomerOnCaseHandler> _logger;

    public UpdateIdentifiedCustomerOnCaseHandler(
        ICustomerServiceAbstraction customerService,
        ICaseServiceAbstraction caseService,
        ILogger<UpdateIdentifiedCustomerOnCaseHandler> logger)
    {
        _customerService = customerService;
        _caseService = caseService;
        _logger = logger;
    }
}
