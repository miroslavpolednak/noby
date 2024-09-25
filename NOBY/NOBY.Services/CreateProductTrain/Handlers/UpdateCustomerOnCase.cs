using DomainServices.HouseholdService.Clients.v1;
using _Case = DomainServices.CaseService.Contracts;
using DomainServices.CaseService.Clients.v1;

namespace NOBY.Services.CreateProductTrain.Handlers;

[ScopedService, SelfService]
internal sealed class UpdateCustomerOnCase(
    ICustomerOnSAServiceClient _customerService,
    ICaseServiceClient _caseService)
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
}
