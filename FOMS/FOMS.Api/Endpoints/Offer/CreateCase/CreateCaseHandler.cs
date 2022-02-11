using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class CreateCaseHandler
    : IRequestHandler<Dto.CreateCaseRequest, Dto.CreateCaseResponse>
{
    public async Task<Dto.CreateCaseResponse> Handle(Dto.CreateCaseRequest request, CancellationToken cancellationToken)
    {
        // detail simulace
        var offerInstance = ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(request.OfferId, cancellationToken));

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
            ProductTypeId = offerInstance.ProductTypeId
        }, cancellationToken);

        return new Dto.CreateCaseResponse
        {
            SalesArrangementId = salesArrangementId,
            CaseId = caseId
        };
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateCaseHandler> _logger;

    public CreateCaseHandler(IOfferServiceAbstraction offerService, ILogger<CreateCaseHandler> logger, IMediator mediator)
    {
        _mediator = mediator;
        _logger = logger;
        _offerService = offerService;
    }
}
