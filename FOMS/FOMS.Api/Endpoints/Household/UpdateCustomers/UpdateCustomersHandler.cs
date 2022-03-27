using CIS.Foms.Enums;
using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateCustomers;

internal class UpdateCustomersHandler
    : IRequestHandler<UpdateCustomersRequest, UpdateCustomersResponse>
{
    public async Task<UpdateCustomersResponse> Handle(UpdateCustomersRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateCustomersHandler), request.HouseholdId);

        // detail domacnosti
        var householdInstance = ServiceCallResult.Resolve<_SA.Household>(await _householdService.GetHousehold(request.HouseholdId, cancellationToken));

        var response = new UpdateCustomersResponse
        {
            CustomerOnSAId1 = await processCustomer(request.Customer1, householdInstance.CustomerOnSAId1, householdInstance, CustomerRoles.Debtor, cancellationToken),
            CustomerOnSAId2 = await processCustomer(request.Customer2, householdInstance.CustomerOnSAId2, householdInstance, CustomerRoles.Codebtor, cancellationToken),
        };

        // linkovani na household
        if (householdInstance.CustomerOnSAId1 != response.CustomerOnSAId1 || householdInstance.CustomerOnSAId2 != response.CustomerOnSAId2)
            await _householdService.LinkCustomerOnSAToHousehold(householdInstance.HouseholdId, response.CustomerOnSAId1, response.CustomerOnSAId2, cancellationToken);

        return response;
    }

    async Task<int?> processCustomer(
        CustomerDto? customer,
        int? householdCustomerId,
        _SA.Household householdInstance,
        CustomerRoles customerRole,
        CancellationToken cancellationToken)
    {
        // zalozit, updatovat nebo smazat customera
        int? customerId = await crudCustomer(customer, householdCustomerId, householdInstance, customerRole, cancellationToken);

        if (customerId.HasValue)
        {
            // ulozit obligations
            var obligationsRequest = new _SA.UpdateObligationsRequest
            {
                CustomerOnSAId = customerId.Value
            };
            if (customer!.Obligations is not null)
                obligationsRequest.Obligations.AddRange(customer.Obligations.ToDomainServiceRequest());
            await _customerOnSAService.UpdateObligations(obligationsRequest, cancellationToken);

            // ulozit incomes
            await _mediator.Send(new CustomerIncome.UpdateIncomes.UpdateIncomesRequest
            {
                CustomerOnSAId = customerId.Value,
                Incomes = customer?.Incomes
            }, cancellationToken);
        }

        return customerId;
    }

    async Task<int?> crudCustomer(
        CustomerDto? customer, 
        int? householdCustomerId,
        _SA.Household householdInstance,
        CustomerRoles customerRole,
        CancellationToken cancellationToken)
    {
        // smazat existujiciho, neni misto nej zadny novy
        if ((customer is null || customer.CustomerOnSAId.GetValueOrDefault() == 0) && householdCustomerId.HasValue)
        {
            await _customerOnSAService.DeleteCustomer(householdCustomerId.Value, cancellationToken);
            return default(int?);
        }
        else if (customer is not null)
        {
            int? newMpId = null;
            int? customerId = customer.CustomerOnSAId;

            // smazat existujiciho, je nahrazen novym
            if (customer.CustomerOnSAId != householdCustomerId && householdCustomerId.HasValue)
                await _customerOnSAService.DeleteCustomer(householdCustomerId.Value, cancellationToken);

            // update stavajiciho
            if (customer.CustomerOnSAId.HasValue)
            {
                newMpId = ServiceCallResult.Resolve<_SA.UpdateCustomerResponse>(await _customerOnSAService.UpdateCustomer(new _SA.UpdateCustomerRequest
                {
                    CustomerOnSAId = customer.CustomerOnSAId!.Value,
                    Customer = customer.ToDomainServiceRequest()
                }, cancellationToken)).PartnerId;
            }
            else // vytvoreni noveho
            {
                var createResult = ServiceCallResult.Resolve<_SA.CreateCustomerResponse>(await _customerOnSAService.CreateCustomer(new _SA.CreateCustomerRequest
                {
                    SalesArrangementId = householdInstance.SalesArrangementId,
                    Customer = customer.ToDomainServiceRequest()
                }, cancellationToken));
                newMpId = createResult.PartnerId;
                customerId = createResult.CustomerOnSAId;
            }

            // hlavni domacnost - hlavni klient ma modre ID
            if (customerRole == CustomerRoles.Debtor && householdInstance.HouseholdTypeId == (int)HouseholdTypes.Main)
            {
                var notification = new Notifications.MainCustomerUpdatedNotification(householdInstance.CaseId, householdInstance.SalesArrangementId, customerId!.Value, newMpId);
                await _mediator.Publish(notification, cancellationToken);
            }

            return customerId;
        }
        return null;
    }

    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly ILogger<UpdateCustomersHandler> _logger;
    private readonly IMediator _mediator;

    public UpdateCustomersHandler(
        IMediator mediator,
        IHouseholdServiceAbstraction householdService,
        ICustomerOnSAServiceAbstraction customerOnSAService,
        ILogger<UpdateCustomersHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
    }
}
