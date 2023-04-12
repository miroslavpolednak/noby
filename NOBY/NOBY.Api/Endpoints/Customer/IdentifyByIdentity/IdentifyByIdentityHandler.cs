using DomainServices.SalesArrangementService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.CustomerService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

internal sealed class IdentifyByIdentityHandler
    : IRequestHandler<IdentifyByIdentityRequest>
{
    public async Task Handle(IdentifyByIdentityRequest request, CancellationToken cancellationToken)
    {
        // customer On SA
        var customerOnSaInstance = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);

        // validate two same identities on household
        if (customerOnSaInstance.CustomerIdentifiers?.Any() ?? false)
        {
            var customersInHousehold = await _customerOnSAService.GetCustomerList(customerOnSaInstance.SalesArrangementId, cancellationToken);
            foreach (var customer in customersInHousehold.Where(t => t.CustomerOnSAId != customerOnSaInstance.CustomerOnSAId))
            {
                var customerDetail = await _customerOnSAService.GetCustomer(customer.CustomerOnSAId, cancellationToken);
                if (customerOnSaInstance.CustomerIdentifiers.Any(x => customerDetail.CustomerIdentifiers.Any(t => t.IdentityScheme == x.IdentityScheme && t.IdentityId == x.IdentityId)))
                {
                    throw new NobyValidationException("Identity already present on SalesArrangement customers");
                }
            }
        }

        // crm customer
        var customerInstance = await _customerService.GetCustomerDetail(request.CustomerIdentity!, cancellationToken);        
        // SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(customerOnSaInstance.SalesArrangementId, cancellationToken);

        if (customerOnSaInstance.CustomerIdentifiers is not null && customerOnSaInstance.CustomerIdentifiers.Any())
            throw new NobyValidationException("CustomerOnSA has been already identified");

        var modelToUpdate = new _HO.UpdateCustomerRequest
        {
            CustomerOnSAId = request.CustomerOnSAId,
            Customer = new _HO.CustomerOnSABase
            {
                DateOfBirthNaturalPerson = customerInstance.NaturalPerson.DateOfBirth,
                FirstNameNaturalPerson = customerInstance.NaturalPerson.FirstName,
                Name = customerInstance.NaturalPerson.LastName,
                LockedIncomeDateTime = customerOnSaInstance.LockedIncomeDateTime,
                MaritalStatusId = customerInstance.NaturalPerson?.MaritalStatusStateId
            }
        };
        modelToUpdate.Customer.CustomerIdentifiers.Add(request.CustomerIdentity!);

        var updateResult = await _customerOnSAService.UpdateCustomer(modelToUpdate, cancellationToken);
        
        // hlavni klient
        if (customerOnSaInstance.CustomerRoleId == 1)
        {
            var notification = new Notifications.MainCustomerUpdatedNotification(saInstance.CaseId, saInstance.SalesArrangementId, modelToUpdate.CustomerOnSAId, updateResult.CustomerIdentifiers);
            await _mediator.Publish(notification, cancellationToken);
        }
    }

    private readonly IMediator _mediator;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public IdentifyByIdentityHandler(
        IMediator mediator,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerServiceClient customerService,
        ICustomerOnSAServiceClient customerOnSAService)
    {
        _mediator = mediator;
        _salesArrangementService = salesArrangementService;
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
    }
}
