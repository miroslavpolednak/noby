using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class CreateCaseHandler
    : IRequestHandler<Dto.CreateCaseRequest, Dto.CreateCaseResponse>
{
    public async Task<Dto.CreateCaseResponse> Handle(Dto.CreateCaseRequest request, CancellationToken cancellationToken)
    {
        // detail simulace
        var offerInstance = ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.GetBuildingSavingsDataResponse>(await _offerService.GetBuildingSavingsData(request.OfferInstanceId, cancellationToken));

        // vytvorit case
        long caseId = await _mediator.Send(new SharedHandlers.Requests.SharedCreateCaseRequest
        {
            OfferInstanceId = offerInstance.OfferInstanceId,
            DateOfBirth = request.DateOfBirth,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Customer = request.Customer,
            ProductInstanceTypeId = offerInstance,
            TargetAmount = offerInstance.InputData.TargetAmount
        }, cancellationToken);

        // vytvorit zadost
        int salesArrangementId = await _mediator.Send(new SharedHandlers.Requests.SharedCreateSalesArrangementRequest
        {
            CaseId = caseId,
            OfferInstanceId = request.OfferInstanceId,
            ProductInstanceTypeId = _configuration.BuildingSavings.SavingsSalesArrangementType
        }, cancellationToken);

        return new Dto.CreateCaseResponse
        {
            SalesArrangementId = 1
        };
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly IMediator _mediator;
    private readonly ILogger<SimulateMortgageHandler> _logger;

    public CreateCaseHandler(IOfferServiceAbstraction offerService, ILogger<SimulateMortgageHandler> logger, IMediator mediator)
    {
        _mediator = mediator;
        _logger = logger;
        _offerService = offerService;
    }
}
