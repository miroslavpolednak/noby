using DomainServices.OfferService.Clients;
using DomainServices.SalesArrangementService.Clients;
using DSContract = DomainServices.OfferService.Contracts;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgage;

internal class SimulateMortgageHandler
    : IRequestHandler<SimulateMortgageRequest, SimulateMortgageResponse>
{
    public async Task<SimulateMortgageResponse> Handle(SimulateMortgageRequest request, CancellationToken cancellationToken)
    {
        // datum garance
        DateTime guaranteeDateFrom;
        if (!request.WithGuarantee.GetValueOrDefault())
            guaranteeDateFrom = DateTime.Now;
        else
        {
            if (!request.SalesArrangementId.HasValue)
                throw new CisValidationException("withGuarantee=true, but SalesArrangementId is not set");

            var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId.Value, cancellationToken);
            guaranteeDateFrom = saInstance.OfferGuaranteeDateFrom;
        }

        // predelat na DS request
        var model = request.ToDomainServiceRequest(guaranteeDateFrom);

        // zavolat DS
        try
        {
            var result = await _offerService.SimulateMortgage(model, cancellationToken);

            return new()
            {
                OfferId = result.OfferId,
                ResourceProcessId = result.ResourceProcessId,
                SimulationResults = result.SimulationResults.ToApiResponse(model.SimulationInputs, result.AdditionalSimulationResults)
            };
        }
        catch (CisArgumentException ex)
        {
            // rethrow to be catched by validation middleware
            throw new CisValidationException(ex.ExceptionCode, ex.Message);
        }
    }
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IOfferServiceClient _offerService;
    
    public SimulateMortgageHandler(IOfferServiceClient offerService, ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
    }
}
