using DomainServices.CodebookService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.Offer.CreateMortgageCase;

internal class CreateMortgageCaseHandler
    : IRequestHandler<CreateMortgageCaseRequest, CreateMortgageCaseResponse>
{
    public async Task<CreateMortgageCaseResponse> Handle(CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        // detail simulace
        var offerInstance = ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(request.OfferId, cancellationToken));

        if (!ServiceCallResult.IsEmptyResult(await _salesArrangementService.GetSalesArrangementByOfferId(offerInstance.OfferId)))
            throw new CisValidationException(ErrorCodes.OfferIdAlreadyLinkedToSalesArrangement, $"OfferId {request.OfferId} has been already linked to another contract");
        
        // get default saTypeId from productTypeId
        int salesArrangementTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken))
            .FirstOrDefault(t => t.ProductTypeId == offerInstance.ProductTypeId && t.IsDefault)
            ?.Id ?? throw new CisNotFoundException(ErrorCodes.OfferDefaultSalesArrangementTypeIdNotFound, $"Default SalesArrangementTypeId for ProductTypeId {offerInstance.ProductTypeId} not found");
        
        // vytvorit case
        long caseId = await _mediator.Send(new SharedHandlers.Requests.SharedCreateCaseRequest
        {
            OfferId = offerInstance.OfferId,
            DateOfBirth = request.DateOfBirth,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Customer = request.Customer,
            ProductTypeId = offerInstance.ProductTypeId,
            TargetAmount = offerInstance.Inputs.LoanAmount
        }, cancellationToken);

        // vytvorit zadost
        int salesArrangementId = await _mediator.Send(new SharedHandlers.Requests.SharedCreateSalesArrangementRequest
        {
            CaseId = caseId,
            OfferId = request.OfferId,
            SalesArrangementTypeId = salesArrangementTypeId
        }, cancellationToken);

        return new CreateMortgageCaseResponse
        {
            SalesArrangementId = salesArrangementId,
            CaseId = caseId,
            OfferId = offerInstance.OfferId
        };
    }

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateMortgageCaseHandler> _logger;

    public CreateMortgageCaseHandler(
        ISalesArrangementServiceAbstraction salesArrangementService, 
        ICodebookServiceAbstraction codebookService, 
        IOfferServiceAbstraction offerService, 
        ILogger<CreateMortgageCaseHandler> logger, IMediator mediator)
    {
        _salesArrangementService = salesArrangementService;
        _codebookService = codebookService;
        _mediator = mediator;
        _logger = logger;
        _offerService = offerService;
    }
}
