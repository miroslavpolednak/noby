using DomainServices.HouseholdService.Clients;
using _Case = DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Notifications.Handlers;

internal class UpdateCustomerOnCaseHandler
    : INotificationHandler<MainCustomerUpdatedNotification>
{
    public async Task Handle(MainCustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        // detail customera
        var customerInstance = await _customerService.GetCustomer(notification.CustomerOnSAId, cancellationToken);

        if (customerInstance.CustomerRoleId == (int)CIS.Foms.Enums.CustomerRoles.Debtor)
        {
            // update case detailu
            await _caseService.UpdateCustomerData(notification.CaseId, new _Case.CustomerData
            {
                DateOfBirthNaturalPerson = customerInstance.DateOfBirthNaturalPerson,
                FirstNameNaturalPerson = customerInstance.FirstNameNaturalPerson,
                Name = customerInstance.Name,
                Identity = customerInstance.CustomerIdentifiers?.FirstOrDefault()
            }, cancellationToken);
        }
    }

    private readonly ICaseServiceClient _caseService;
    private readonly ICustomerOnSAServiceClient _customerService;

    public UpdateCustomerOnCaseHandler(
        ICustomerOnSAServiceClient customerService,
        ICaseServiceClient caseService)
    {
        _customerService = customerService;
        _caseService = caseService;
    }
}
