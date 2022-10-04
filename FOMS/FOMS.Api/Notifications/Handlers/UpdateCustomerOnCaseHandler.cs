using DomainServices.HouseholdService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Case = DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Abstraction;
using _HO = DomainServices.HouseholdService.Contracts;

namespace FOMS.Api.Notifications.Handlers;

internal class UpdateCustomerOnCaseHandler
    : INotificationHandler<MainCustomerUpdatedNotification>
{
    public async Task Handle(MainCustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        // detail customera
        var customerInstance = ServiceCallResult.ResolveAndThrowIfError<_HO.CustomerOnSA>(await _customerService.GetCustomer(notification.CustomerOnSAId, cancellationToken));

        // update case detailu
        await _caseService.UpdateCaseCustomer(notification.CaseId, new _Case.CustomerData
        {
            DateOfBirthNaturalPerson = customerInstance.DateOfBirthNaturalPerson,
            FirstNameNaturalPerson = customerInstance.FirstNameNaturalPerson,
            Name = customerInstance.Name,
            Identity = customerInstance.CustomerIdentifiers?.FirstOrDefault()
        }, cancellationToken);
    }

    private readonly ICaseServiceAbstraction _caseService;
    private readonly ICustomerOnSAServiceClient _customerService;
    private readonly ILogger<UpdateCustomerOnCaseHandler> _logger;

    public UpdateCustomerOnCaseHandler(
        ICustomerOnSAServiceClient customerService,
        ICaseServiceAbstraction caseService,
        ILogger<UpdateCustomerOnCaseHandler> logger)
    {
        _customerService = customerService;
        _caseService = caseService;
        _logger = logger;
    }
}
