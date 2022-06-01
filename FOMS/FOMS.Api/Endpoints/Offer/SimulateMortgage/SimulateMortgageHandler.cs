using DomainServices.OfferService.Abstraction;
using DSContract = DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.Offer.SimulateMortgage;

internal class SimulateMortgageHandler
    : IRequestHandler<SimulateMortgageRequest, SimulateMortgageResponse>
{
    public async Task<SimulateMortgageResponse> Handle(SimulateMortgageRequest request, CancellationToken cancellationToken)
    {
        _logger.OfferSimulateMortgageStarted(request);

        // predelat na DS request
        var model = request.ToDomainServiceRequest();
        
        // zavolat DS
        var result = await callOfferService(model, cancellationToken);

        // predelat z DS na FE Dto
        SimulateMortgageResponse responseModel = new()
        {
            OfferId = result.OfferId,
            ResourceProcessId = result.ResourceProcessId,
            SimulationResults = result.SimulationResults.ToApiResponse(model.SimulationInputs)
        };
        
        _logger.OfferSimulateMortgageResult(responseModel);

        return responseModel;
    }

    private async Task<DSContract.SimulateMortgageResponse> callOfferService(DSContract.SimulateMortgageRequest model, CancellationToken cancellationToken)
    {
        try
        {
            return ServiceCallResult.ResolveAndThrowIfError<DSContract.SimulateMortgageResponse>(await _offerService.SimulateMortgage(model, cancellationToken));
        }
        catch (CisArgumentException ex)
        {
            // rethrow to be catched by validation middleware
            throw new CisValidationException(ex);
        }
    }

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<SimulateMortgageHandler> _logger;

    public SimulateMortgageHandler(IOfferServiceAbstraction offerService, ILogger<SimulateMortgageHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
