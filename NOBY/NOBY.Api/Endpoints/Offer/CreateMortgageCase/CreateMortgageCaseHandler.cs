using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.CaseService.Clients;
using _Case = DomainServices.CaseService.Contracts;
using _Offer = DomainServices.OfferService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _HO = DomainServices.HouseholdService.Contracts;
using CIS.Infrastructure.MediatR.Rollback;

namespace NOBY.Api.Endpoints.Offer.CreateMortgageCase;

internal class CreateMortgageCaseHandler
    : IRequestHandler<CreateMortgageCaseRequest, CreateMortgageCaseResponse>
{
    public async Task<CreateMortgageCaseResponse> Handle(CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        // detail simulace
        var offerInstance = await _offerService.GetMortgageOffer(request.OfferId, cancellationToken);

        // chyba pokud simulace je uz nalinkovana na jiny SA
        if (!ServiceCallResult.IsEmptyResult(await _salesArrangementService.GetSalesArrangementByOfferId(offerInstance.OfferId, cancellationToken)))
            throw new CisValidationException(ErrorCodes.OfferIdAlreadyLinkedToSalesArrangement, $"OfferId {request.OfferId} has been already linked to another contract");
        
        // get default saTypeId from productTypeId
        int salesArrangementTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .FirstOrDefault(t => t.ProductTypeId == offerInstance.SimulationInputs.ProductTypeId)
            ?.Id ?? throw new CisNotFoundException(ErrorCodes.OfferDefaultSalesArrangementTypeIdNotFound, $"Default SalesArrangementTypeId for ProductTypeId {offerInstance.SimulationInputs.ProductTypeId} not found");

        // vytvorit case
        _logger.SharedCreateCaseStarted(offerInstance.OfferId);
        long caseId = await _caseService.CreateCase(request.ToDomainServiceRequest(_userAccessor.User!.Id, offerInstance.SimulationInputs), cancellationToken);
        _bag.Add(CreateMortgageCaseRollback.BagKeyCaseId, caseId);
        _logger.EntityCreated(nameof(_Case.Case), caseId);

        // updatovat kontakty
        await _caseService.UpdateOfferContacts(caseId, new _Case.OfferContacts
        {
            EmailForOffer = request.EmailForOffer ?? "",
            PhoneNumberForOffer = request.PhoneNumberForOffer ?? ""
        }, cancellationToken);

        // vytvorit zadost
        _logger.SharedCreateSalesArrangementStarted(salesArrangementTypeId, caseId, request.OfferId);
        int salesArrangementId = ServiceCallResult.ResolveAndThrowIfError<int>(await _salesArrangementService.CreateSalesArrangement(caseId, salesArrangementTypeId, request.OfferId, cancellationToken));
        _bag.Add(CreateMortgageCaseRollback.BagKeySalesArrangementId, salesArrangementId);
        _logger.EntityCreated(nameof(_SA.SalesArrangement), salesArrangementId);

        // create customer on SA
        var createCustomerResult = await _customerOnSAService.CreateCustomer(request.ToDomainServiceRequest(salesArrangementId), cancellationToken);
        _bag.Add(CreateMortgageCaseRollback.BagKeyCustomerOnSAId, createCustomerResult.CustomerOnSAId);
        
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

    private readonly IRollbackBag _bag;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICodebookServiceClients _codebookService;
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
        ICustomerOnSAServiceClient customerOnSAService,
        ISalesArrangementServiceClient salesArrangementService,
        IHouseholdServiceClient householdService,
        ICaseServiceClient caseService,
        ICodebookServiceClients codebookService, 
        IOfferServiceClient offerService, 
        ILogger<CreateMortgageCaseHandler> logger)
    {
        _bag = bag;
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
