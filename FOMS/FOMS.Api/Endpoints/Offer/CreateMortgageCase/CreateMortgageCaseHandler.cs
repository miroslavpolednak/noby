using DomainServices.CodebookService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.CaseService.Abstraction;
using _Case = DomainServices.CaseService.Contracts;
using _Offer = DomainServices.OfferService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Offer.CreateMortgageCase;

internal class CreateMortgageCaseHandler
    : IRequestHandler<CreateMortgageCaseRequest, CreateMortgageCaseResponse>
{
    public async Task<CreateMortgageCaseResponse> Handle(CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        // detail simulace
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<_Offer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(request.OfferId, cancellationToken));

        // chyba pokud simulace je uz nalinkovana na jiny SA
        if (!ServiceCallResult.IsEmptyResult(await _salesArrangementService.GetSalesArrangementByOfferId(offerInstance.OfferId, cancellationToken)))
            throw new CisValidationException(ErrorCodes.OfferIdAlreadyLinkedToSalesArrangement, $"OfferId {request.OfferId} has been already linked to another contract");
        
        // get default saTypeId from productTypeId
        int salesArrangementTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .FirstOrDefault(t => t.ProductTypeId == offerInstance.SimulationInputs.ProductTypeId && t.IsDefault)
            ?.Id ?? throw new CisNotFoundException(ErrorCodes.OfferDefaultSalesArrangementTypeIdNotFound, $"Default SalesArrangementTypeId for ProductTypeId {offerInstance.SimulationInputs.ProductTypeId} not found");

        // vytvorit case
        _logger.SharedCreateCaseStarted(offerInstance.OfferId);
        long caseId = ServiceCallResult.ResolveAndThrowIfError<long>(await _caseService.CreateCase(request.ToDomainServiceRequest(_userAccessor.User!.Id, offerInstance.SimulationInputs), cancellationToken));
        _logger.EntityCreated(nameof(_Case.Case), caseId);

        // vytvorit zadost
        _logger.SharedCreateSalesArrangementStarted(salesArrangementTypeId, caseId, request.OfferId);
        int salesArrangementId = ServiceCallResult.ResolveAndThrowIfError<int>(await _salesArrangementService.CreateSalesArrangement(caseId, salesArrangementTypeId, request.OfferId, cancellationToken));
        _logger.EntityCreated(nameof(_SA.SalesArrangement), salesArrangementId);

        // create customer on SA
        var createCustomerResult = ServiceCallResult.ResolveAndThrowIfError<_SA.CreateCustomerResponse>(await _customerOnSAService.CreateCustomer(request.ToDomainServiceRequest(salesArrangementId), cancellationToken));
        
        // create household
        int householdId = ServiceCallResult.ResolveAndThrowIfError<int>(await _householdService.CreateHousehold(new _SA.CreateHouseholdRequest
        {
            HouseholdTypeId = (int)CIS.Foms.Enums.HouseholdTypes.Main,
            CustomerOnSAId1 = createCustomerResult.CustomerOnSAId,
            SalesArrangementId = salesArrangementId
        }, cancellationToken));
        _logger.EntityCreated(nameof(Household), householdId);

        // mam identifikovaneho customera
        if (createCustomerResult.PartnerId.HasValue)
        {
            var notification = new Notifications.MainCustomerUpdatedNotification(caseId, salesArrangementId, createCustomerResult.CustomerOnSAId, createCustomerResult.PartnerId.Value, _userAccessor.User?.Id);
            await _mediator.Publish(notification, cancellationToken);
        }

        //TODO co udelat, kdyz se neco z toho nepovede?

        return new CreateMortgageCaseResponse
        {
            SalesArrangementId = salesArrangementId,
            CaseId = caseId,
            OfferId = offerInstance.OfferId,
            HouseholdId = householdId,
            CustomerOnSAId = createCustomerResult.CustomerOnSAId
        };
    }

    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<CreateMortgageCaseHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly IMediator _mediator;

    public CreateMortgageCaseHandler(
        IMediator mediator,
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ICustomerOnSAServiceAbstraction customerOnSAService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        IHouseholdServiceAbstraction householdService,
        ICaseServiceAbstraction caseService,
        ICodebookServiceAbstraction codebookService, 
        IOfferServiceAbstraction offerService, 
        ILogger<CreateMortgageCaseHandler> logger)
    {
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
