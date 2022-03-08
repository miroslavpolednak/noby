using DomainServices.OfferService.Abstraction;
using DSContracts = DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.Offer.GetMortgageByOfferId;

internal class GetMortgageByOfferIdHandler
    : IRequestHandler<GetMortgageByOfferIdRequest, Dto.GetMortgageResponse>
{
    public async Task<Dto.GetMortgageResponse> Handle(GetMortgageByOfferIdRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetMortgageByOfferIdHandler), request.OfferId);

        var result = ServiceCallResult.Resolve<DSContracts.GetMortgageDataResponse>(await _offerService.GetMortgageData(request.OfferId, cancellationToken));

        _logger.RequestHandlerFinished(nameof(GetMortgageByOfferIdHandler));

        // predelat z DS na FE Dto
        return new()
        {
            OfferId = result.OfferId,
            ResourceProcessId = result.ResourceProcessId,
            Inputs = result.Inputs.ToApiResponse(),
            Outputs = result.Outputs.ToApiResponse()
        };
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<GetMortgageByOfferIdHandler> _logger;

    public GetMortgageByOfferIdHandler(IOfferServiceAbstraction offerService, ILogger<GetMortgageByOfferIdHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
