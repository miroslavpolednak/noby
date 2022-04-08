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

        // Honza chtel Obligations v rootu requestu
        if (request.Obligations is not null && request.Customer1 is not null)
            request.Customer1!.Obligations = convertObligations(request.Obligations, 0);
        if (request.Obligations is not null && request.Customer2 is not null)
            request.Customer2!.Obligations = convertObligations(request.Obligations, 1);

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

    static List<Dto.CustomerObligation>? convertObligations(List<Dto.HouseholdCustomerObligation>? list, int index)
        =>list?.Where(t => t.CustomerIndex == index).Select(t => new Dto.CustomerObligation
        {
            CreditCardLimit = t.CreditCardLimit,
            LoanPaymentAmount = t.LoanPaymentAmount,
            ObligationCreditor = t.ObligationCreditor,
            ObligationTypeId = t.ObligationTypeId,
            RemainingLoanPrincipal = t.RemainingLoanPrincipal
        }).ToList();

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
        if (customer is null && householdCustomerId.HasValue)
        {
            await _customerOnSAService.DeleteCustomer(householdCustomerId.Value, cancellationToken);
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
                try
                {
                    newMpId = ServiceCallResult.Resolve<_SA.UpdateCustomerResponse>(await _customerOnSAService.UpdateCustomer(new _SA.UpdateCustomerRequest
                    {
                        CustomerOnSAId = customer.CustomerOnSAId!.Value,
                        Customer = customer.ToDomainServiceRequest()
                    }, cancellationToken)).PartnerId;
                }
                catch (CisArgumentException ex) when (ex.ExceptionCode == 16033)
                {
                    // osetrena vyjimka, kdy je klient identifikovan KB identitou, ale nepodarilo se vytvorit identitu v MP
                    //TODO je otazka, jak se zde zachovat?
                }
            }
            else // vytvoreni noveho
            {
                var createResult = ServiceCallResult.Resolve<_SA.CreateCustomerResponse>(await _customerOnSAService.CreateCustomer(new _SA.CreateCustomerRequest
                {
                    SalesArrangementId = householdInstance.SalesArrangementId,
                    CustomerRoleId = (int)customerRole,
                    Customer = customer.ToDomainServiceRequest()
                }, cancellationToken));
                newMpId = createResult.PartnerId;
                customerId = createResult.CustomerOnSAId;
            }

            // hlavni domacnost - hlavni klient ma modre ID
            if (newMpId.HasValue && customerRole == CustomerRoles.Debtor && householdInstance.HouseholdTypeId == (int)HouseholdTypes.Main)
            {
                var notification = new Notifications.MainCustomerUpdatedNotification(householdInstance.CaseId, householdInstance.SalesArrangementId, customerId!.Value, newMpId);
                await _mediator.Publish(notification, cancellationToken);
            }

            return customerId;
        }
        return default(int?);
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
