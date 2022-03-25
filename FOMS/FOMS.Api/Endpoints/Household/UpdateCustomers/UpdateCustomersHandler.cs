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
        if ((customer is null || customer?.CustomerOnSAId.GetValueOrDefault() == 0) && householdCustomerId.HasValue)
        {
            await _customerOnSAService.DeleteCustomer(householdCustomerId.Value, cancellationToken);
            return default(int?);
        }
        // smazat existujiciho, je nahrazen novym
        else if (customer?.CustomerOnSAId != householdCustomerId && customer?.CustomerOnSAId != null &&  householdCustomerId.HasValue)
        {
            await _customerOnSAService.DeleteCustomer(householdCustomerId.Value, cancellationToken);
            return customer.CustomerOnSAId;
        }
        // zalozit noveho
        else if (customer?.Identity?.Id > 0)
        {
            var createCustomerResult = await _createCustomerService.Create(householdInstance.SalesArrangementId, customer, cancellationToken);
            _logger.EntityCreated(nameof(_SA.CustomerOnSA), createCustomerResult.CustomerOnSAId);

            // hlavni domacnost - hlavni klient ma modre ID
            if (createCustomerResult.PartnerId.HasValue && householdInstance.HouseholdTypeId == (int)CIS.Foms.Enums.HouseholdTypes.Debtor)
            {
                var notification = new Notifications.CustomerFullyIdentifiedNotification(householdInstance.CaseId, householdInstance.SalesArrangementId, customer.Identity, createCustomerResult.PartnerId.Value);
                await _mediator.Publish(notification, cancellationToken);
            }

            return createCustomerResult.CustomerOnSAId;
        }

        return customer?.CustomerOnSAId;
    }

    private readonly CreateCustomerService _createCustomerService;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly ILogger<UpdateCustomersHandler> _logger;
    private readonly IMediator _mediator;

    public UpdateCustomersHandler(
        IMediator mediator,
        CreateCustomerService createCustomerService,
        IHouseholdServiceAbstraction householdService,
        ICustomerOnSAServiceAbstraction customerOnSAService,
        ILogger<UpdateCustomersHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _createCustomerService = createCustomerService;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
    }
}
