using DomainServices.SalesArrangementService.Abstraction;
using contracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

internal class UpdateHouseholdHandler
    : IRequestHandler<UpdateHouseholdRequest, UpdateHouseholdResponse>
{
    public async Task<UpdateHouseholdResponse> Handle(UpdateHouseholdRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateHouseholdHandler), request.HouseholdId);

        // nacist ulozenou domacnost
        var household = ServiceCallResult.Resolve<contracts.Household>(await _householdService.GetHousehold(request.HouseholdId, cancellationToken));

        // update customeru
        int? customerId1 = await createOrUpdateCustomer(request.Customer1, household, household.CustomerOnSAId1, cancellationToken);
        int? customerId2 = await createOrUpdateCustomer(request.Customer2, household, household.CustomerOnSAId2, cancellationToken);

        // update domacnosti
        var householdRequest = new DomainServices.SalesArrangementService.Contracts.UpdateHouseholdRequest
        {
            HouseholdId = request.HouseholdId,
            CustomerOnSAId1 = customerId1,
            CustomerOnSAId2 = customerId2,
            Data = request.Data.MapToRequest(),
            Expenses = request.Expenses.MapToRequest()
        };
        await _householdService.UpdateHousehold(householdRequest, cancellationToken);

        return new UpdateHouseholdResponse
        {
            HouseholdId = request.HouseholdId,
            CustomerOnSAId1 = customerId1,
            CustomerOnSAId2 = customerId2
        };
    }

    async Task<int?> createOrUpdateCustomer(Dto.CustomerInHousehold? customer, contracts.Household household, int? householdCustomerOnSAId, CancellationToken cancellationToken)
    {
        bool householdCustomerExists = householdCustomerOnSAId.GetValueOrDefault() > 0;
        int? customerId = null;

        if (customer is null && householdCustomerExists) // smazat puvodniho customera
        {
            await _customerOnSAService.DeleteCustomer(householdCustomerOnSAId!.Value, cancellationToken);
        }
        else if (customer is not null)
        {
            if (householdCustomerExists && householdCustomerOnSAId == customer.CustomerOnSAId) // update existujiciho customera
            {
                await _customerOnSAService.UpdateCustomer(customer.MapToRequest(), cancellationToken);
                customerId = customer.CustomerOnSAId;
            }
            else // vytvorit noveho customera
            {
                customerId = ServiceCallResult.Resolve<int>(await _customerOnSAService.CreateCustomer(customer.MapToRequest(household.SalesArrangementId), cancellationToken));
            }

            // smazat puvodniho, pokud je jiny nez aktualni
            if (householdCustomerExists && customer.CustomerOnSAId != householdCustomerOnSAId)
                await _customerOnSAService.DeleteCustomer(householdCustomerOnSAId!.Value, cancellationToken);
        }

        return customerId;
    }

    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly ILogger<UpdateHouseholdHandler> _logger;

    public UpdateHouseholdHandler(
        IHouseholdServiceAbstraction householdService,
        ICustomerOnSAServiceAbstraction customerOnSAService,
        ILogger<UpdateHouseholdHandler> logger)
    {
        _logger = logger;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
    }
}
