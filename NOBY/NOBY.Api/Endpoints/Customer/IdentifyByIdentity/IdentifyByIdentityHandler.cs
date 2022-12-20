using DomainServices.SalesArrangementService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.CustomerService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

internal sealed class IdentifyByIdentityHandler
    : AsyncRequestHandler<IdentifyByIdentityRequest>
{
    protected override async Task Handle(IdentifyByIdentityRequest request, CancellationToken cancellationToken)
    {
        // crm customer
        var customerInstance = await _customerService.GetCustomerDetail(request.CustomerIdentity!, cancellationToken);
        // customer On SA
        var customerOnSaInstance = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);
        // SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(customerOnSaInstance.SalesArrangementId, cancellationToken));

        if (customerOnSaInstance.CustomerIdentifiers is not null && customerOnSaInstance.CustomerIdentifiers.Any())
            throw new CisValidationException(0, "CustomerOnSA has been already identified");

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
