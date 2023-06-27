using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.CaseService.Clients;
using _Case = DomainServices.CaseService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _HO = DomainServices.HouseholdService.Contracts;
using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.CustomerService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Offer.CreateMortgageCase;

internal sealed class CreateMortgageCaseHandler
    : IRequestHandler<CreateMortgageCaseRequest, CreateMortgageCaseResponse>
{
    public async Task<CreateMortgageCaseResponse> Handle(CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        // detail simulace
        var offerInstance = await _offerService.GetMortgageOffer(request.OfferId, cancellationToken);

        // chyba pokud simulace je uz nalinkovana na jiny SA
        if (await _salesArrangementService.GetSalesArrangementByOfferId(offerInstance.OfferId, cancellationToken) is not null)
            throw new NobyValidationException($"OfferId {request.OfferId} has been already linked to another contract");

        // get default saTypeId from productTypeId
        int salesArrangementTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .First(t => t.SalesArrangementCategory == 1)
            .Id;

        // vytvorit case
        _logger.SharedCreateCaseStarted(offerInstance.OfferId);
        long caseId = await _caseService.CreateCase(request.ToDomainServiceRequest(_userAccessor.User!.Id, offerInstance.SimulationInputs), cancellationToken);
        _bag.Add(CreateMortgageCaseRollback.BagKeyCaseId, caseId);
        _logger.EntityCreated(nameof(_Case.Case), caseId);

        // updatovat kontakty
        await _caseService.UpdateOfferContacts(caseId, new _Case.OfferContacts
        {
            EmailForOffer = request.OfferContacts?.EmailAddress?.EmailAddress ?? "",
            PhoneNumberForOffer = new()
            {
                PhoneNumber = request.OfferContacts?.MobilePhone?.PhoneNumber ?? "",
                PhoneIDC = request.OfferContacts?.MobilePhone?.PhoneIDC ?? ""
            }
        }, cancellationToken);

        // vytvorit zadost
        _logger.SharedCreateSalesArrangementStarted(salesArrangementTypeId, caseId, request.OfferId);
        int salesArrangementId = await _salesArrangementService.CreateSalesArrangement(caseId, salesArrangementTypeId, request.OfferId, cancellationToken);
        _bag.Add(CreateMortgageCaseRollback.BagKeySalesArrangementId, salesArrangementId);
        _logger.EntityCreated(nameof(_SA.SalesArrangement), salesArrangementId);

        // pokud je to KB klient, tak si stahni jeho data z CM a updatuj request
        var createCustomerRequest = request.ToDomainServiceRequest(salesArrangementId);
        if (request.Identity?.Scheme == CIS.Foms.Enums.IdentitySchemes.Kb)
        {
            await updateCustomerFromCM(createCustomerRequest, cancellationToken);
        }
        // create customer on SA
        var createCustomerResult = await _customerOnSAService.CreateCustomer(createCustomerRequest, cancellationToken);
        _bag.Add(CreateMortgageCaseRollback.BagKeyCustomerOnSAId, createCustomerResult.CustomerOnSAId);

        // updatovat Agent v SA parameters, vytvarime prazdny objekt Parameters pouze s agentem
        await updateSalesArrangementParameters(salesArrangementId, createCustomerResult.CustomerOnSAId, cancellationToken);

        // create household
        int householdId = await _householdService.CreateHousehold(new _HO.CreateHouseholdRequest
        {
            HouseholdTypeId = (int)CIS.Foms.Enums.HouseholdTypes.Main,
            CustomerOnSAId1 = createCustomerResult.CustomerOnSAId,
            SalesArrangementId = salesArrangementId
        }, cancellationToken);
        _bag.Add(CreateMortgageCaseRollback.BagKeyHouseholdId, householdId);
        _logger.EntityCreated(nameof(Household), householdId);

        // mam identifikovaneho customera
        var notification = new Notifications.MainCustomerUpdatedNotification(caseId, salesArrangementId, createCustomerResult.CustomerOnSAId, createCustomerResult.CustomerIdentifiers);
        await _mediator.Publish(notification, cancellationToken);
        
        return new CreateMortgageCaseResponse
        {
            SalesArrangementId = salesArrangementId,
            CaseId = caseId,
            OfferId = offerInstance.OfferId,
            HouseholdId = householdId,
            CustomerOnSAId = createCustomerResult.CustomerOnSAId
        };
    }

    /// <summary>
    /// Update parametru na SA -> musime si je znovu stahnout z nove vytvoreneho SA, protoze mohou obsahovat nejake defaulty doplnene domenovou sluzbou
    /// </summary>
    private async Task updateSalesArrangementParameters(int salesArrangementId, int customerOnSAId, CancellationToken cancellationToken)
    {
        // stahnout aktualni verzi parametru
        var saInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);

        // doplnit Agent
        var mortgage = saInstance.Mortgage ?? new SalesArrangementParametersMortgage();
        mortgage.Agent = customerOnSAId;

        await _salesArrangementService.UpdateSalesArrangementParameters(new _SA.UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangementId,
            Mortgage = mortgage
        }, cancellationToken);
    }

    private async Task updateCustomerFromCM(_HO.CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerDetail(request.Customer.CustomerIdentifiers[0], cancellationToken);

        if (!string.IsNullOrEmpty(customer.NaturalPerson.FirstName))
            request.Customer.FirstNameNaturalPerson = customer.NaturalPerson.FirstName;
        if (!string.IsNullOrEmpty(customer.NaturalPerson.LastName))
            request.Customer.Name = customer.NaturalPerson.LastName;
        if (customer.NaturalPerson.DateOfBirth is not null)
            request.Customer.DateOfBirthNaturalPerson = customer.NaturalPerson.DateOfBirth;
        request.Customer.MaritalStatusId = customer.NaturalPerson.MaritalStatusStateId;
    }

    private readonly IRollbackBag _bag;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICaseServiceClient _caseService;
    private readonly IOfferServiceClient _offerService;
    private readonly ILogger<CreateMortgageCaseHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly IMediator _mediator;

    public CreateMortgageCaseHandler(
        IRollbackBag bag,
        IMediator mediator,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ICustomerServiceClient customerService,
        ICustomerOnSAServiceClient customerOnSAService,
        ISalesArrangementServiceClient salesArrangementService,
        IHouseholdServiceClient householdService,
        ICaseServiceClient caseService,
        ICodebookServiceClient codebookService, 
        IOfferServiceClient offerService, 
        ILogger<CreateMortgageCaseHandler> logger)
    {
        _bag = bag;
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
        _mediator = mediator;
        _userAccessor = userAccessor;
        _caseService = caseService;
        _householdService = householdService;
        _salesArrangementService = salesArrangementService;
        _codebookService = codebookService;
        _logger = logger;
        _offerService = offerService;
    }
}
