using DomainServices.OfferService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.OfferService.Api.Endpoints;

[Authorize]
public class OfferService 
    : Contracts.v1.OfferService.OfferServiceBase
{
    public override async Task<ValidateOfferIdResponse> ValidateOfferId(ValidateOfferIdRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetOfferResponse> GetOffer(GetOfferRequest request, ServerCallContext context)
    => await _mediator.Send(request);

    public override async Task<GetOfferDetailResponse> GetOfferDetail(GetOfferDetailRequest request, ServerCallContext context)
       => await _mediator.Send(request);

    public override async Task<SimulateMortgageResponse> SimulateMortgage(SimulateMortgageRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetMortgageOfferFPScheduleResponse> GetMortgageOfferFPSchedule(GetMortgageOfferFPScheduleRequest request, ServerCallContext context)
      => await _mediator.Send(request);

    public override async Task<GetOfferDeveloperResponse> GetOfferDeveloper(GetOfferDeveloperRequest request, ServerCallContext context)
      => await _mediator.Send(request);

    public override async Task<Empty> UpdateOfferDocumentId(UpdateOfferDocumentIdRequest request, ServerCallContext context)
    {
        await _mediator.Send(request);
        return new Empty();
    }

    private readonly IMediator _mediator;
    public OfferService(IMediator mediator)
        => _mediator = mediator;
}
