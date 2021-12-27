using CIS.Core.Exceptions;
using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer.Handlers;

internal class SimulateHandler 
    : IRequestHandler<SimulateRequest, OfferInstance>
{
    public async Task<OfferInstance> Handle(SimulateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Simulate savings {request}", request);

        var model = new DomainServices.OfferService.Contracts.SimulateBuildingSavingsRequest
        {
            ResourceProcessId = request.ResourceProcessId,
            InputData = request
        };
        var result = resolveResult(await _offerService.SimulateBuildingSavings(model, cancellationToken));

        _logger.LogDebug("OfferInstanceId #{id} created", result.OfferInstanceId);

        return new OfferInstance(result.OfferInstanceId, request, result.BuildingSavings);
    }

    private DomainServices.OfferService.Contracts.SimulateBuildingSavingsResponse resolveResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<DomainServices.OfferService.Contracts.SimulateBuildingSavingsResponse> r => r.Model,
            SimulationServiceErrorResult e1 when e1.Errors.Count() == 1 => throw new CisValidationException(e1.Errors.First().Message),
            SimulationServiceErrorResult e1 when e1.Errors.Count() > 1 => throw new CisValidationException(e1.Errors),
            ErrorServiceCallResult e2 => throw new CisValidationException(e2.Errors),
            _ => throw new NotImplementedException()
        };

    private readonly IOfferServiceAbstraction _offerService;
    private readonly ILogger<SimulateHandler> _logger;
        
    public SimulateHandler(IOfferServiceAbstraction offerService, ILogger<SimulateHandler> logger)
    {
        _logger = logger;
        _offerService = offerService;
    }
}
