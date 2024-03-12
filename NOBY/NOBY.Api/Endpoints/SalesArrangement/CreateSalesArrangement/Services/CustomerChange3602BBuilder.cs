using SharedTypes.Enums;
using DomainServices.HouseholdService.Contracts;
using __SA = DomainServices.SalesArrangementService.Contracts;
using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class CustomerChange3602BBuilder
    : BaseBuilder
{
    public override async Task PostCreateProcessing(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        var salesArrangementService = GetRequiredService<DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient>();
        var customerOnSAService = GetRequiredService<DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient>();
        var householdService = GetRequiredService<DomainServices.HouseholdService.Clients.IHouseholdServiceClient>();

        // vytvorit domacnost
        var requestModel = new CreateHouseholdRequest
        {
            SalesArrangementId = salesArrangementId,
            HouseholdTypeId = (int)HouseholdTypes.Codebtor
        };

        var householdId = await householdService.CreateHousehold(requestModel, cancellationToken);

        // vytvorit klienta
        var createCustomerResult = await customerOnSAService.CreateCustomer(new CreateCustomerRequest
        {
            CustomerRoleId = (int)CustomerRoles.Codebtor,
            SalesArrangementId = salesArrangementId
        }, cancellationToken);

        await householdService.LinkCustomerOnSAToHousehold(householdId, createCustomerResult.CustomerOnSAId, null, cancellationToken);

        // update parametru
        await salesArrangementService.UpdateSalesArrangementParameters(new()
        {
            SalesArrangementId = salesArrangementId,
            CustomerChange3602B = new()
            {
                HouseholdId = householdId
            }
        }, cancellationToken);
    }

    public CustomerChange3602BBuilder(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }
}
