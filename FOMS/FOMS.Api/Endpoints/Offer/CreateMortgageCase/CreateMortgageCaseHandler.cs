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
        var offerInstance = ServiceCallResult.Resolve<_Offer.GetMortgageDataResponse>(await _offerService.GetMortgageData(request.OfferId, cancellationToken));

        // chyba pokud simulace je uz nalinkovana na jiny SA
        if (!ServiceCallResult.IsEmptyResult(await _salesArrangementService.GetSalesArrangementByOfferId(offerInstance.OfferId, cancellationToken)))
            throw new CisValidationException(ErrorCodes.OfferIdAlreadyLinkedToSalesArrangement, $"OfferId {request.OfferId} has been already linked to another contract");
        
        // get default saTypeId from productTypeId
        int salesArrangementTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .FirstOrDefault(t => t.ProductTypeId == offerInstance.ProductTypeId && t.IsDefault)
            ?.Id ?? throw new CisNotFoundException(ErrorCodes.OfferDefaultSalesArrangementTypeIdNotFound, $"Default SalesArrangementTypeId for ProductTypeId {offerInstance.ProductTypeId} not found");

        // vytvorit case
        _logger.SharedCreateCaseStarted(offerInstance.OfferId);
        long caseId = ServiceCallResult.Resolve<long>(await _caseService.CreateCase(getCreateCaseRequest(request, offerInstance.Inputs), cancellationToken));
        _logger.EntityCreated(nameof(_Case.Case), caseId);

        // vytvorit zadost
        _logger.SharedCreateSalesArrangementStarted(salesArrangementTypeId, caseId, request.OfferId);
        int salesArrangementId = ServiceCallResult.Resolve<int>(await _salesArrangementService.CreateSalesArrangement(caseId, salesArrangementTypeId, request.OfferId, cancellationToken));
        _logger.EntityCreated(nameof(_SA.SalesArrangement), salesArrangementId);

        // create household
        int householdId = ServiceCallResult.Resolve<int>(await _householdService.CreateHousehold(new _SA.CreateHouseholdRequest
        {
            HouseholdTypeId = (int)CIS.Foms.Enums.HouseholdTypes.Main,
            SalesArrangementId = salesArrangementId
        }, cancellationToken));
        _logger.EntityCreated(nameof(Household), householdId);

        // create household and customer on SA
        var createCustomerResult = await _createCustomerService.Create(salesArrangementId, request, cancellationToken);
        if (createCustomerResult.PartnerId.HasValue)
        {
            var notification = new Notifications.CustomerFullyIdentifiedNotification(caseId, salesArrangementId, request.Identity!, createCustomerResult.PartnerId.Value);
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

    /// <summary>
    /// Vytvoreni requestu pro zalozeni CASE
    /// </summary>
    _Case.CreateCaseRequest getCreateCaseRequest(CreateMortgageCaseRequest request, _Offer.MortgageInput offerInstance)
        => new _Case.CreateCaseRequest
        {
            CaseOwnerUserId = _userAccessor.User.Id,
            Customer = new _Case.CustomerData
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName,
                Name = request.LastName,
                Identity = request.Identity is null ? null : new CIS.Infrastructure.gRPC.CisTypes.Identity(request.Identity)
            },
            Data = new _Case.CaseData
            {
                ProductTypeId = offerInstance.ProductTypeId,
                TargetAmount = offerInstance.LoanAmount
            }
        };

    private readonly CreateCustomerService _createCustomerService;
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
        CreateCustomerService createCustomerService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        IHouseholdServiceAbstraction householdService,
        ICaseServiceAbstraction caseService,
        ICodebookServiceAbstraction codebookService, 
        IOfferServiceAbstraction offerService, 
        ILogger<CreateMortgageCaseHandler> logger)
    {
        _mediator = mediator;
        _userAccessor = userAccessor;
        _caseService = caseService;
        _householdService = householdService;
        _createCustomerService = createCustomerService;
        _salesArrangementService = salesArrangementService;
        _codebookService = codebookService;
        _logger = logger;
        _offerService = offerService;
    }
}
