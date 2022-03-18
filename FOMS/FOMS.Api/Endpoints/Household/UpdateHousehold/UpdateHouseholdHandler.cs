using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

internal class UpdateHouseholdHandler
    : IRequestHandler<UpdateHouseholdRequest, UpdateHouseholdResponse>
{
    public async Task<UpdateHouseholdResponse> Handle(UpdateHouseholdRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateHouseholdHandler), request.HouseholdId);

        // nacist ulozenou domacnost
        var household = ServiceCallResult.Resolve<_SA.Household>(await _householdService.GetHousehold(request.HouseholdId, cancellationToken));

        // detail SA
        var saInstance = ServiceCallResult.Resolve<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(household.SalesArrangementId, cancellationToken));

        // update customeru
        int? customerId1 = await createOrUpdateCustomer(request.Customer1, household, household.CustomerOnSAId1, cancellationToken);
        int? customerId2 = await createOrUpdateCustomer(request.Customer2, household, household.CustomerOnSAId2, cancellationToken);

        // update domacnosti
        var householdRequest = new _SA.UpdateHouseholdRequest
        {
            HouseholdId = request.HouseholdId,
            CustomerOnSAId1 = customerId1,
            CustomerOnSAId2 = customerId2,
            Data = request.Data.ToDomainServiceRequest(),
            Expenses = request.Expenses.ToDomainServiceRequest()
        };
        await _householdService.UpdateHousehold(householdRequest, cancellationToken);

        // hlavni domacnost - hlavni klient ma modre ID
        if (customerId1.HasValue && household.HouseholdTypeId == (int)CIS.Foms.Enums.HouseholdTypes.Debtor)
        {
            var notification = new Notifications.CustomerFullyIdentifiedNotification(saInstance.CaseId, household.SalesArrangementId, request.Customer1!.Identities!.First(), customerId1.Value);
            await _mediator.Publish(notification, cancellationToken);
        }

        return new UpdateHouseholdResponse
        {
            HouseholdId = request.HouseholdId,
            CustomerOnSAId1 = customerId1,
            CustomerOnSAId2 = customerId2
        };
    }

    async Task<int?> createOrUpdateCustomer(Dto.CustomerInHousehold? customer, _SA.Household household, int? householdCustomerOnSAId, CancellationToken cancellationToken)
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
                await _customerOnSAService.UpdateCustomer(customer.ToDomainServiceRequest(), cancellationToken);
                customerId = customer.CustomerOnSAId;
            }
            else // vytvorit noveho customera
            {
                customerId = ServiceCallResult.Resolve<int>(await _customerOnSAService.CreateCustomer(customer.ToDomainServiceRequest(household.SalesArrangementId), cancellationToken));
            }

            // smazat puvodniho, pokud je jiny nez aktualni
            if (householdCustomerExists && customer.CustomerOnSAId != householdCustomerOnSAId)
                await _customerOnSAService.DeleteCustomer(householdCustomerOnSAId!.Value, cancellationToken);
        }

        return customerId;
    }

    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly ILogger<UpdateHouseholdHandler> _logger;
    private readonly Mediator _mediator;

    public UpdateHouseholdHandler(
        Mediator mediator,
        ISalesArrangementServiceAbstraction salesArrangementService,
        IHouseholdServiceAbstraction householdService,
        ICustomerOnSAServiceAbstraction customerOnSAService,
        ILogger<UpdateHouseholdHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _salesArrangementService = salesArrangementService;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
    }
}
