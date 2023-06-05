using CIS.Foms.Enums;
using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class CustomerChange3602BBuilder
    : BaseBuilder
{
    public override async Task PostCreateProcessing(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        var mediator = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<IMediator>();
        var salesArrangementService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient>();
        var customerOnSAService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient>();
        var householdService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.HouseholdService.Clients.IHouseholdServiceClient>();

        // zalozit household
        var householdResult = await mediator.Send(new Endpoints.Household.CreateHousehold.CreateHouseholdRequest
        {
            SalesArrangementId = salesArrangementId,
            HouseholdTypeId = (int)HouseholdTypes.Codebtor,
            HardCreate = true
        }, cancellationToken);

        // vytvorit klienta
        var createCustomerResult = await customerOnSAService.CreateCustomer(new DomainServices.HouseholdService.Contracts.CreateCustomerRequest
        {
            CustomerRoleId = (int)CustomerRoles.Codebtor,
            SalesArrangementId = salesArrangementId
        }, cancellationToken);

        await householdService.LinkCustomerOnSAToHousehold(householdResult.HouseholdId, createCustomerResult.CustomerOnSAId, null, cancellationToken);

        // update parametru
        await salesArrangementService.UpdateSalesArrangementParameters(new()
        {
            SalesArrangementId = salesArrangementId,
            CustomerChange3602A = new()
            {
                HouseholdId = householdResult.HouseholdId
            }
        }, cancellationToken);
    }

    public CustomerChange3602BBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, __SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request, httpContextAccessor)
    {
    }
}
