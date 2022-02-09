using CIS.Core.Results;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class UpdateDraftHandler
    : IRequestHandler<UpdateDraftRequest, SaveCaseResponse>
{
    public async Task<SaveCaseResponse> Handle(UpdateDraftRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Update SA #{salesArrangementId} for {offerInstanceId}", request.SalesArrangementId, request.OfferInstanceId);

        // get SA instance
        var saInstance = ServiceCallResult.Resolve<DomainServices.SalesArrangementService.Contracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        // dotahnout informace o offerInstance
        var offerInstance = ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(request.OfferInstanceId, cancellationToken));

        // nalinkovat novou simulaci na SA
        ServiceCallResult.Resolve(await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferInstanceId, cancellationToken));

        // update case data
        await _mediator.Publish(new Notifications.Requests.CaseDataUpdatedRequest
        {
            CaseId = saInstance.CaseId,
            //TargetAmount = offerInstance.InputData.TargetAmount
        }, cancellationToken);

        // update dat klienta na Case
        await _mediator.Publish(new Notifications.Requests.CaseCustomerUpdatedRequest
        {
            CaseId = saInstance.CaseId,
            DateOfBirthNaturalPerson = request.DateOfBirth,
            FirstNameNaturalPerson = request.FirstName,
            Name = request.LastName
        });

        return new SaveCaseResponse
        {
            CaseId = saInstance.CaseId,
            SalesArrangementId = request.SalesArrangementId
        };
    }

    private readonly IMediator _mediator;
    private readonly ILogger<UpdateDraftHandler> _logger;
    private readonly DomainServices.OfferService.Abstraction.IOfferServiceAbstraction _offerService;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;

    public UpdateDraftHandler(
        ILogger<UpdateDraftHandler> logger, 
        IMediator mediator, 
        DomainServices.OfferService.Abstraction.IOfferServiceAbstraction offerService, 
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
        _mediator = mediator;
        _logger = logger;
    }
}
