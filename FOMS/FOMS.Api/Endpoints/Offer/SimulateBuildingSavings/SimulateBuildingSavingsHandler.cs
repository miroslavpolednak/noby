using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Offer.Dto;

namespace FOMS.Api.Endpoints.Offer.Handlers;

internal class SimulateBuildingSavingsHandler 
    : IRequestHandler<SimulateBuildingSavingsRequest, OfferInstance>
{
    private readonly IOfferServiceAbstraction _offerService;
        
    public SimulateBuildingSavingsHandler(IOfferServiceAbstraction offerService)
    {
        _offerService = offerService;
    }

    public async Task<OfferInstance> Handle(SimulateBuildingSavingsRequest request, CancellationToken cancellationToken)
    {
        var result = resolveResult(await _offerService.SimulateBuildingSavings(request));

        return new OfferInstance(result.OfferInstanceId, request, result.BuildingSavings);
    }

    private DomainServices.OfferService.Contracts.SimulateBuildingSavingsResponse resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.OfferService.Contracts.SimulateBuildingSavingsResponse> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            ErrorServiceCallResult e2 => throw new CIS.Core.Exceptions.CisValidationException(e2.Errors),
            _ => throw new NotImplementedException()
        };
}
