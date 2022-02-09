using CIS.Core.Results;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class CreateDraftHandler
    : IRequestHandler<CreateDraftRequest, SaveCaseResponse>
{
    public async Task<SaveCaseResponse> Handle(CreateDraftRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Create building savings case (draft) for {offerInstanceId}", request.OfferInstanceId);

        // detail simulace
        var offerInstance = ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(request.OfferInstanceId, cancellationToken));
        
        // vytvorit case
        long caseId = await _mediator.Send(new SharedHandlers.Requests.SharedCreateCaseRequest
        {
            OfferInstanceId = request.OfferInstanceId,
            DateOfBirth = request.DateOfBirth,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Customer = request.Customer,
            //ProductInstanceTypeId = _configuration.BuildingSavings.SavingsProductInstanceType,
            //TargetAmount = offerInstance.InputData.TargetAmount
        }, cancellationToken);

        // vytvorit zadost
        int salesArrangementId = await _mediator.Send(new SharedHandlers.Requests.SharedCreateSalesArrangementRequest
        {
            CaseId = caseId,
            OfferInstanceId = request.OfferInstanceId,
            ProductTypeId = _configuration.BuildingSavings.SavingsSalesArrangementTypeId
        }, cancellationToken);
        
        return new SaveCaseResponse
        {
            CaseId = caseId,
            SalesArrangementId = salesArrangementId
        };
    }

    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly ILogger<CreateDraftHandler> _logger;
    private readonly IMediator _mediator;
    private readonly Infrastructure.Configuration.AppConfiguration _configuration;

    public CreateDraftHandler(
        ILogger<CreateDraftHandler> logger,
        IMediator mediator,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService,
        Infrastructure.Configuration.AppConfiguration configuration)
    {
        _offerService = offerService;
        _configuration = configuration;
        _mediator = mediator;
        _logger = logger;
    }
}
