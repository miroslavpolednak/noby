﻿using DomainServices.OfferService.Abstraction;
using DSContracts = DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.Offer.GetMortgageByOfferId;

internal class GetMortgageByOfferIdHandler
    : IRequestHandler<GetMortgageByOfferIdRequest, Dto.GetMortgageResponse>
{
    public async Task<Dto.GetMortgageResponse> Handle(GetMortgageByOfferIdRequest request, CancellationToken cancellationToken)
    {
        var result = ServiceCallResult.ResolveAndThrowIfError<DSContracts.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(request.OfferId, cancellationToken));

        _logger.RequestHandlerFinished(nameof(GetMortgageByOfferIdHandler));

        // predelat z DS na FE Dto
        return new()
        {
            OfferId = result.OfferId,
            ResourceProcessId = result.ResourceProcessId,
            Inputs = result.SimulationInputs.ToApiResponse(result.BasicParameters),
            Outputs = result.SimulationResults.ToApiResponse(result.SimulationInputs)
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
