using DomainServices.HouseholdService.Clients;
using _Case = DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Clients.v1;

namespace NOBY.Services.CreateProductTrain.Handlers;

[ScopedService, SelfService]
internal sealed class UpdateCustomerOnCase
{
    public async Task Run(long caseId, int customerOnSAId, CancellationToken cancellationToken)
    {
        // detail customera
        var customerInstance = await _customerService.GetCustomer(customerOnSAId, cancellationToken);

        if (customerInstance.CustomerRoleId == (int)EnumCustomerRoles.Debtor)
        {
            // update case detailu
            await _caseService.UpdateCustomerData(caseId, new _Case.CustomerData
            {
                DateOfBirthNaturalPerson = customerInstance.DateOfBirthNaturalPerson,
                FirstNameNaturalPerson = customerInstance.FirstNameNaturalPerson,
                Name = customerInstance.Name,
                Identity = customerInstance.CustomerIdentifiers.FirstOrDefault()
            }, cancellationToken);
        }
    }

    private readonly ICaseServiceClient _caseService;
    private readonly ICustomerOnSAServiceClient _customerService;

    public UpdateCustomerOnCase(
        ICustomerOnSAServiceClient customerService,
        ICaseServiceClient caseService)
    {
        _customerService = customerService;
        _caseService = caseService;
    }
}
