using DomainServices.OfferService.Abstraction;
using DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class SimulateMortgageHandler
    : IRequestHandler<Dto.SimulateMortgageRequest, Dto.SimulateMortgageResponse>
{
    public async Task<Dto.SimulateMortgageResponse> Handle(Dto.SimulateMortgageRequest request, CancellationToken cancellationToken)
    {
        _logger.SimulateMortgageStarted(request);

        // predelat na DS request
        var model = request.ToDomainServiceRequest();
        
        // zavolat DS
        var result = ServiceCallResult.Resolve<SimulateMortgageResponse>(await _offerService.SimulateMortgage(model, cancellationToken));

        // predelat z DS na FE Dto
        Dto.SimulateMortgageResponse responseModel = new()
        {
            OfferId = result.OfferId,
            ResourceProcessId = result.ResourceProcessId,
            Outputs = result.Outputs.ToResponseDto()
        };
        
        _logger.SimulateMortgageResult(responseModel);

        return responseModel;
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<SimulateMortgageHandler> _logger;

    public SimulateMortgageHandler(IOfferServiceAbstraction offerService, ILogger<SimulateMortgageHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
