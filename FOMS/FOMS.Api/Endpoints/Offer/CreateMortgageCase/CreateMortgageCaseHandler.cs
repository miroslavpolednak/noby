using DomainServices.CodebookService.Abstraction;
using DomainServices.ProductService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.CaseService.Abstraction;
using caseContracts = DomainServices.CaseService.Contracts;
using offerContracts = DomainServices.OfferService.Contracts;
using saContracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Offer.CreateMortgageCase;

internal class CreateMortgageCaseHandler
    : IRequestHandler<CreateMortgageCaseRequest, CreateMortgageCaseResponse>
{
    public async Task<CreateMortgageCaseResponse> Handle(CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        // detail simulace
        var offerInstance = ServiceCallResult.Resolve<offerContracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(request.OfferId, cancellationToken));

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
        _logger.EntityCreated(nameof(caseContracts.Case), caseId);

        // vytvorit zadost
        _logger.SharedCreateSalesArrangementStarted(salesArrangementTypeId, caseId, request.OfferId);
        int salesArrangementId = ServiceCallResult.Resolve<int>(await _salesArrangementService.CreateSalesArrangement(caseId, salesArrangementTypeId, request.OfferId, cancellationToken));
        _logger.EntityCreated(nameof(saContracts.SalesArrangement), salesArrangementId);
        
        // create household and customer on SA
        var householdCustomerResult = await _createCustomerWithHouseholdService.Create(salesArrangementId, request, cancellationToken);

        // zalozit produkt, pokud ma klient modre ID
        if (householdCustomerResult.PartnerId.HasValue)
            await _createProductService.CreateMortgage(caseId, offerInstance, cancellationToken);

        //TODO co udelat, kdyz se neco z toho nepovede?

        return new CreateMortgageCaseResponse
        {
            SalesArrangementId = salesArrangementId,
            CaseId = caseId,
            OfferId = offerInstance.OfferId,
            CustomerOnSAId = householdCustomerResult.CustomerOnSAId,
            HouseholdId = householdCustomerResult.HouseholdId
        };
    }

    /// <summary>
    /// Vytvoreni requestu pro zalozeni CASE
    /// </summary>
    caseContracts.CreateCaseRequest getCreateCaseRequest(CreateMortgageCaseRequest request, offerContracts.MortgageInput offerInstance)
        => new caseContracts.CreateCaseRequest
        {
            CaseOwnerUserId = _userAccessor.User.Id,
            Customer = new caseContracts.CustomerData
            {
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName,
                Name = request.LastName,
                Identity = request.Customer is null ? null : new CIS.Infrastructure.gRPC.CisTypes.Identity(request.Customer)
            },
            Data = new caseContracts.CaseData
            {
                ProductTypeId = offerInstance.ProductTypeId,
                TargetAmount = offerInstance.LoanAmount
            }
        };

    private readonly CreateCustomerWithHouseholdService _createCustomerWithHouseholdService;
    private readonly CreateProductService _createProductService;
    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<CreateMortgageCaseHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public CreateMortgageCaseHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        CreateProductService createProductService,
        CreateCustomerWithHouseholdService createCustomerWithHouseholdService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        ICaseServiceAbstraction caseService,
        ICodebookServiceAbstraction codebookService, 
        IOfferServiceAbstraction offerService, 
        ILogger<CreateMortgageCaseHandler> logger)
    {
        _createProductService = createProductService;
        _userAccessor = userAccessor;
        _caseService = caseService;
        _createCustomerWithHouseholdService = createCustomerWithHouseholdService;
        _salesArrangementService = salesArrangementService;
        _codebookService = codebookService;
        _logger = logger;
        _offerService = offerService;
    }
}
