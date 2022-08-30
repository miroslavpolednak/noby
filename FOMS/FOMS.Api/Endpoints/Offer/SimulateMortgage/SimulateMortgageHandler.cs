using DomainServices.OfferService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using DSContract = DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.Offer.SimulateMortgage;

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

            var saInstance = ServiceCallResult.ResolveAndThrowIfError<DomainServices.SalesArrangementService.Contracts.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId.Value, cancellationToken));
            guaranteeDateFrom = saInstance.OfferGuaranteeDateFrom;
        }

        // predelat na DS request
        var model = request.ToDomainServiceRequest(guaranteeDateFrom);
        
        // zavolat DS
        var result = await callOfferService(model, cancellationToken);
        
        // predelat z DS na FE Dto
        SimulateMortgageResponse responseModel = new()
        {
            OfferId = result.OfferId,
            ResourceProcessId = result.ResourceProcessId,
            SimulationResults = result.SimulationResults.ToApiResponse(model.SimulationInputs, result.AdditionalSimulationResults)
        };

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

    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly IOfferServiceAbstraction _offerService;
    
    public SimulateMortgageHandler(IOfferServiceAbstraction offerService, ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
        _offerService = offerService;
    }
}
