using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _CS = DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.Customer.IdentifyByIdentity;

internal sealed class IdentifyByIdentityHandler
    : AsyncRequestHandler<IdentifyByIdentityRequest>
{
    protected override async Task Handle(IdentifyByIdentityRequest request, CancellationToken cancellationToken)
    {
        // crm customer
        var customerInstance = ServiceCallResult.ResolveAndThrowIfError<_CS.CustomerDetailResponse>(await _customerService.GetCustomerDetail(request.CustomerIdentity!, cancellationToken));
        // customer On SA
        var customerOnSaInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.CustomerOnSA>(await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken));
        // SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(customerOnSaInstance.SalesArrangementId, cancellationToken));

        if (customerOnSaInstance.CustomerIdentifiers is not null && customerOnSaInstance.CustomerIdentifiers.Any())
            throw new CisValidationException(0, "CustomerOnSA has been already identified");

        var modelToUpdate = new _SA.UpdateCustomerRequest
        {
            CustomerOnSAId = request.CustomerOnSAId,
            Customer = new _SA.CustomerOnSABase
            {
                DateOfBirthNaturalPerson = customerInstance.NaturalPerson.DateOfBirth,
                FirstNameNaturalPerson = customerInstance.NaturalPerson.FirstName,
                Name = customerInstance.NaturalPerson.LastName,
                LockedIncomeDateTime = customerOnSaInstance.LockedIncomeDateTime
            }
        };
        modelToUpdate.Customer.CustomerIdentifiers.Add(request.CustomerIdentity!);

        var successfulUpdate = ServiceCallResult.IsSuccessResult(await _customerOnSAService.UpdateCustomer(modelToUpdate, cancellationToken));

        // hlavni klient
        if (successfulUpdate && customerOnSaInstance.CustomerRoleId == 1)
        {
            // update CASE-u
            var updateResponse = ServiceCallResult.Resolve(await _caseService.UpdateCaseCustomer(saInstance.CaseId, new DomainServices.CaseService.Contracts.CustomerData
            {
                Identity = request.CustomerIdentity!,
                DateOfBirthNaturalPerson = customerInstance.NaturalPerson.DateOfBirth,
                FirstNameNaturalPerson = customerInstance.NaturalPerson.FirstName,
                Name = customerInstance.NaturalPerson.LastName
            }, cancellationToken));

            if (customerInstance.Identity.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp)
            {
                var mpid = customerInstance.Identity.IdentityId;
                var notification = new Notifications.MainCustomerUpdatedNotification(saInstance.CaseId, saInstance.SalesArrangementId, modelToUpdate.CustomerOnSAId, mpid);
                await _mediator.Publish(notification, cancellationToken);
            }
        }
    }

    private readonly IMediator _mediator;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;

    public IdentifyByIdentityHandler(
        IMediator mediator,
        ISalesArrangementServiceAbstraction salesArrangementService,
        ICaseServiceAbstraction caseService,
        ICustomerServiceAbstraction customerService,
        ICustomerOnSAServiceAbstraction customerOnSAService)
    {
        _mediator = mediator;
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
    }
}
