using DomainServices.OfferService.Api.Dto;
using DomainServices.OfferService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.OfferService.Api.Services;

[Authorize]
public class OfferService : Contracts.v1.OfferService.OfferServiceBase
{
    private readonly IMediator _mediator;

    public OfferService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<GetOfferResponse> GetOffer(OfferIdRequest request, ServerCallContext context)
    => await _mediator.Send(new GetOfferMediatrRequest(request));

    public override async Task<GetMortgageDataResponse> GetMortgageData(OfferIdRequest request, ServerCallContext context)
       => await _mediator.Send(new GetMortgageDataMediatrRequest(request));

    public override async Task<SimulateMortgageResponse> SimulateMortgage(SimulateMortgageRequest request, ServerCallContext context)
        => await _mediator.Send(new SimulateMortgageMediatrRequest(request));

}
