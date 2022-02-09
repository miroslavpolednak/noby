using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class SimulateMortgageHandler
    : IRequestHandler<Dto.SimulateMortgageRequest, Dto.SimulateMortgageResponse>
{
    public async Task<Dto.SimulateMortgageResponse> Handle(Dto.SimulateMortgageRequest request, CancellationToken cancellationToken)
    {
        _logger.SimulateMortgageStarted(request);

        var model = new DomainServices.OfferService.Contracts.SimulateMortgageRequest        {
            ResourceProcessId = request.ResourceProcessId,
        };
        var result = CIS.Core.Results.ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.SimulateMortgageResponse>(await _offerService.SimulateMortgage(model, cancellationToken));

        //_logger.LogDebug("OfferInstanceId #{id} created", result.OfferInstanceId);

        return new Dto.SimulateMortgageResponse
        {
            /*OfferInstanceId = result.OfferInstanceId,
            ProductInstanceTypeId = result.ProductInstanceTypeId,
            CreatedBy = result.CreatedBy,
            CreatedOn = result.CreatedOn,*/
            ResourceProcessId = request.ResourceProcessId,
            Results = null
        };
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<SimulateMortgageHandler> _logger;

    public SimulateMortgageHandler(IOfferServiceAbstraction offerService, ILogger<SimulateMortgageHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
