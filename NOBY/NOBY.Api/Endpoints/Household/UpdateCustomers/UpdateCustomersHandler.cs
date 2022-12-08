using CIS.Foms.Enums;
using DomainServices.HouseholdService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.UpdateCustomers;

internal class UpdateCustomersHandler
    : IRequestHandler<UpdateCustomersRequest, UpdateCustomersResponse>
{
    public async Task<UpdateCustomersResponse> Handle(UpdateCustomersRequest request, CancellationToken cancellationToken)
    {
        // detail domacnosti
        var householdInstance = await _householdService.GetHousehold(request.HouseholdId, cancellationToken);
        
        var c1 = await crudCustomer(request.Customer1, householdInstance.CustomerOnSAId1, householdInstance, CustomerRoles.Debtor, cancellationToken);
        var c2 = await crudCustomer(request.Customer2, householdInstance.CustomerOnSAId2, householdInstance, CustomerRoles.Codebtor, cancellationToken);

        // linkovani na household
        if (householdInstance.CustomerOnSAId1 != c1.CustomerOnSAId || householdInstance.CustomerOnSAId2 != c2.CustomerOnSAId)
            await _householdService.LinkCustomerOnSAToHousehold(householdInstance.HouseholdId, c1.CustomerOnSAId, c2.CustomerOnSAId, cancellationToken);

        // hlavni domacnost - hlavni klient ma modre ID
        if (c1.CustomerOnSAId.HasValue && householdInstance.HouseholdTypeId == (int)HouseholdTypes.Main)
        {
            var notification = new Notifications.MainCustomerUpdatedNotification(householdInstance.CaseId, householdInstance.SalesArrangementId, c1.CustomerOnSAId!.Value, c1.Identities);
            await _mediator.Publish(notification, cancellationToken);
        }

        return new UpdateCustomersResponse
        {
            CustomerOnSAId1 = c1.CustomerOnSAId,
            CustomerOnSAId2 = c2.CustomerOnSAId
        };
    }

    async Task<(int? CustomerOnSAId, IEnumerable<CIS.Infrastructure.gRPC.CisTypes.Identity>? Identities)> crudCustomer(
        CustomerDto? customer, 
        int? householdCustomerId,
        _HO.Household householdInstance,
        CustomerRoles customerRole,
        CancellationToken cancellationToken)
    {
        // smazat existujiciho, neni misto nej zadny novy
        if (customer is null && householdCustomerId.HasValue)
        {
            await _customerOnSAService.DeleteCustomer(householdCustomerId.Value, cancellationToken: cancellationToken);
        }
        else if (customer is not null)
        {
            // smazat existujiciho, je nahrazen novym
            if (customer.CustomerOnSAId != householdCustomerId && householdCustomerId.HasValue)
                await _customerOnSAService.DeleteCustomer(householdCustomerId.Value, cancellationToken: cancellationToken);

            // update stavajiciho
            if (customer.CustomerOnSAId.HasValue)
            {
                try
                {
                    var currentCustomerInstance = await _customerOnSAService.GetCustomer(customer.CustomerOnSAId!.Value, cancellationToken);

                    var identities = (await _customerOnSAService.UpdateCustomer(new _HO.UpdateCustomerRequest
                        {
                            CustomerOnSAId = customer.CustomerOnSAId!.Value,
                            Customer = customer.ToDomainServiceRequest(currentCustomerInstance.LockedIncomeDateTime)
                        }, cancellationToken))
                        .CustomerIdentifiers;

                    return (customer.CustomerOnSAId.Value, identities);
                }
                catch (CisArgumentException ex) when (ex.ExceptionCode == 16033)
                {
                    // osetrena vyjimka, kdy je klient identifikovan KB identitou, ale nepodarilo se vytvorit identitu v MP
                    //TODO je otazka, jak se zde zachovat?
                }
            }
            else // vytvoreni noveho
            {
                var createResult = await _customerOnSAService.CreateCustomer(new _HO.CreateCustomerRequest
                {
                    SalesArrangementId = householdInstance.SalesArrangementId,
                    CustomerRoleId = (int)customerRole,
                    Customer = customer.ToDomainServiceRequest()
                }, cancellationToken);

                return (createResult.CustomerOnSAId, createResult.CustomerIdentifiers);
            }
        }

        return (default(int?), default(List<CIS.Infrastructure.gRPC.CisTypes.Identity>?));
    }

    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IMediator _mediator;

    public UpdateCustomersHandler(
        IMediator mediator,
        IHouseholdServiceClient householdService,
        ICustomerOnSAServiceClient customerOnSAService)
    {
        _mediator = mediator;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
    }
}
