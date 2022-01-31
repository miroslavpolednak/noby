using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class GetMortgageHandler
    : IRequestHandler<Dto.GetMortgageRequest, Dto.GetMortgageResponse>
{
    public async Task<Dto.GetMortgageResponse> Handle(Dto.GetMortgageRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetMortgageHandler), request.OfferInstanceId);

        //var result = CIS.Core.Results.ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.SimulateMortgageResponse>(await _offerService.SimulateBuildingSavings(model, cancellationToken));

        //_logger.LogDebug("OfferInstanceId #{id} created", result.OfferInstanceId);

        return new Dto.GetMortgageResponse();
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<GetMortgageHandler> _logger;

    public GetMortgageHandler(IOfferServiceAbstraction offerService, ILogger<GetMortgageHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
