using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _CS = DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.Customer.Identify;

internal sealed class IdentifyHandler
    : AsyncRequestHandler<IdentifyRequest>
{
    protected override async Task Handle(IdentifyRequest request, CancellationToken cancellationToken)
    {
        // crm customer
        var customerInstance = ServiceCallResult.ResolveAndThrowIfError<_CS.CustomerResponse>(await _customerService.GetCustomerDetail(new _CS.CustomerRequest
        {
            Identity = request.CustomerIdentity!
        }, cancellationToken));
        // customer On SA
        var customerOnSaInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.CustomerOnSA>(await _customerOnSAService.GetCustomer(1, cancellationToken));
        // SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(customerOnSaInstance.SalesArrangementId, cancellationToken));

        var modelToUpdate = new _SA.UpdateCustomerRequest
        {
            CustomerOnSAId = request.CustomerOnSAId,
            Customer = new _SA.CustomerOnSABase
            {
                DateOfBirthNaturalPerson = customerInstance.NaturalPerson.DateOfBirth,
                FirstNameNaturalPerson = customerInstance.NaturalPerson.FirstName,
                Name = customerInstance.NaturalPerson.LastName,
                HasPartner = customerOnSaInstance.HasPartner,
                LockedIncomeDateTime = customerOnSaInstance.LockedIncomeDateTime
            }
        };
        if (customerOnSaInstance.Obligations is not null)
            modelToUpdate.Customer.Obligations.AddRange(customerOnSaInstance.Obligations);
        modelToUpdate.Customer.CustomerIdentifiers.Add(request.CustomerIdentity!);

        var successfulUpdate = ServiceCallResult.IsSuccessResult(await _customerOnSAService.UpdateCustomer(modelToUpdate, cancellationToken));

        // hlavni klient
        if (customerOnSaInstance.CustomerRoleId == 1)
        {
            // update CASE-u
            await _caseService.UpdateCaseCustomer(saInstance.CaseId, new DomainServices.CaseService.Contracts.CustomerData
            {
                Identity = request.CustomerIdentity!,
                DateOfBirthNaturalPerson = customerInstance.NaturalPerson.DateOfBirth,
                FirstNameNaturalPerson = customerInstance.NaturalPerson.FirstName,
                Name = customerInstance.NaturalPerson.LastName
            }, cancellationToken);

            var notification = new Notifications.MainCustomerUpdatedNotification(saInstance.CaseId, saInstance.SalesArrangementId, modelToUpdate.CustomerOnSAId, createCustomerResult.PartnerId.Value);
            await _mediator.Publish(notification, cancellationToken);
        }
    }

    private readonly IMediator _mediator;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;

    public IdentifyHandler(
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
