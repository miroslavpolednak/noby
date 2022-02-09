using CIS.Core.Results;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class UpdateCaseHandler
    : IRequestHandler<UpdateCaseRequest, SaveCaseResponse>
{
    public async Task<SaveCaseResponse> Handle(UpdateCaseRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Update building savings case for {request}", request);

        // get SA instance
        var saInstance = ServiceCallResult.Resolve<DomainServices.SalesArrangementService.Contracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        // detail simulace
        var offerInstance = ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(request.OfferInstanceId, cancellationToken));

        // nalinkovat novou simulaci na SA
        ServiceCallResult.Resolve(await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferInstanceId, cancellationToken));

        // update case data
        await _mediator.Publish(new Notifications.Requests.CaseDataUpdatedRequest
        {
            CaseId = saInstance.CaseId,
            //TargetAmount = offerInstance.InputData.TargetAmount
        }, cancellationToken);

        return new SaveCaseResponse
        {
            SalesArrangementId = request.SalesArrangementId,
            CaseId = saInstance.CaseId
        };
    }

    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<UpdateCaseHandler> _logger;
    private readonly IMediator _mediator;

    public UpdateCaseHandler(
        ILogger<UpdateCaseHandler> logger,
        IMediator mediator,
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _logger = logger;
        _mediator = mediator;
    }
}
