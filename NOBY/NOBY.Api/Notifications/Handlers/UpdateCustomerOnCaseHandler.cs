using DomainServices.HouseholdService.Clients;
using _Case = DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Notifications.Handlers;

internal sealed class UpdateCustomerOnCaseHandler
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
                Identity = customerInstance.CustomerIdentifiers.FirstOrDefault()
            }, cancellationToken);
            
            //Customer byl identifikovaný, může se nastavit ContractNumber
            if (customerInstance.CustomerIdentifiers.Any())
                await _salesArrangementService.SetContractNumber(notification.SalesArrangementId, notification.CustomerOnSAId, cancellationToken);

        }
    }

    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerService;

    public UpdateCustomerOnCaseHandler(
        ICustomerOnSAServiceClient customerService,
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _customerService = customerService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
