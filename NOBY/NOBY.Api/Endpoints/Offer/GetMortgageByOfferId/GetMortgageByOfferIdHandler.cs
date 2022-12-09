using DomainServices.OfferService.Clients;
using DSContracts = DomainServices.OfferService.Contracts;

namespace NOBY.Api.Endpoints.Offer.GetMortgageByOfferId;

internal class GetMortgageByOfferIdHandler
    : IRequestHandler<GetMortgageByOfferIdRequest, Dto.GetMortgageResponse>
{
    public async Task<Dto.GetMortgageResponse> Handle(GetMortgageByOfferIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _offerService.GetMortgageOfferDetail(request.OfferId, cancellationToken);

        _logger.RequestHandlerFinished(nameof(GetMortgageByOfferIdHandler));

        // predelat z DS na FE Dto
        return new()
        {
            OfferId = result.OfferId,
            ResourceProcessId = result.ResourceProcessId,
            SimulationInputs = result.SimulationInputs.ToApiResponse(result.BasicParameters),
            SimulationResults = result.SimulationResults.ToApiResponse(result.SimulationInputs, result.AdditionalSimulationResults)
        };
    }

    private readonly IOfferServiceClient _offerService;
    private readonly ILogger<GetMortgageByOfferIdHandler> _logger;

    public GetMortgageByOfferIdHandler(IOfferServiceClient offerService, ILogger<GetMortgageByOfferIdHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
