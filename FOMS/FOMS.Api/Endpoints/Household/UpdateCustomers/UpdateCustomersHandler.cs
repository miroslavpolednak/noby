using CIS.Foms.Enums;
using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateCustomers;

internal class UpdateCustomersHandler
    : IRequestHandler<UpdateCustomersRequest, UpdateCustomersResponse>
{
    public async Task<UpdateCustomersResponse> Handle(UpdateCustomersRequest request, CancellationToken cancellationToken)
    {
        // detail domacnosti
        var householdInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.Household>(await _householdService.GetHousehold(request.HouseholdId, cancellationToken));

        var response = new UpdateCustomersResponse
        {
            CustomerOnSAId1 = await crudCustomer(request.Customer1, householdInstance.CustomerOnSAId1, householdInstance, CustomerRoles.Debtor, cancellationToken),
            CustomerOnSAId2 = await crudCustomer(request.Customer2, householdInstance.CustomerOnSAId2, householdInstance, CustomerRoles.Codebtor, cancellationToken)
        };

        // linkovani na household
        if (householdInstance.CustomerOnSAId1 != response.CustomerOnSAId1 || householdInstance.CustomerOnSAId2 != response.CustomerOnSAId2)
            await _householdService.LinkCustomerOnSAToHousehold(householdInstance.HouseholdId, response.CustomerOnSAId1, response.CustomerOnSAId2, cancellationToken);

        return response;
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
                    var currentCustomerInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.CustomerOnSA>(await _customerOnSAService.GetCustomer(customer.CustomerOnSAId!.Value, cancellationToken));

                    newMpId = ServiceCallResult.ResolveAndThrowIfError<_SA.UpdateCustomerResponse>(await _customerOnSAService.UpdateCustomer(new _SA.UpdateCustomerRequest
                    {
                        CustomerOnSAId = customer.CustomerOnSAId!.Value,
                        Customer = customer.ToDomainServiceRequest(currentCustomerInstance.LockedIncomeDateTime)
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
                var createResult = ServiceCallResult.ResolveAndThrowIfError<_SA.CreateCustomerResponse>(await _customerOnSAService.CreateCustomer(new _SA.CreateCustomerRequest
                {
                    SalesArrangementId = householdInstance.SalesArrangementId,
                    CustomerRoleId = (int)customerRole,
                    Customer = customer.ToDomainServiceRequest(customer.LockedIncome ? DateTime.Now : null)
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
    private readonly IMediator _mediator;

    public UpdateCustomersHandler(
        IMediator mediator,
        IHouseholdServiceAbstraction householdService,
        ICustomerOnSAServiceAbstraction customerOnSAService)
    {
        _mediator = mediator;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
    }
}
