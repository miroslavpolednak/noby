using DomainServices.OfferService.Abstraction;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class GetMortgageHandler
    : IRequestHandler<Dto.GetMortgageRequest, Dto.GetMortgageResponse>
{
    public async Task<Dto.GetMortgageResponse> Handle(Dto.GetMortgageRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetMortgageHandler), request.OfferId);

        var result = ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(request.OfferId, cancellationToken));

        _logger.RequestHandlerFinished(nameof(GetMortgageHandler));

        // predelat z DS na FE Dto
        return new()
        {
            OfferId = result.OfferId,
            ResourceProcessId = result.ResourceProcessId,
            Inputs = result.Inputs.ToResponseDto(),
            Outputs = result.Outputs.ToResponseDto()
        };
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<GetMortgageHandler> _logger;

    public GetMortgageHandler(IOfferServiceAbstraction offerService, ILogger<GetMortgageHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
