using DomainServices.OfferService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.OfferService.Api.Endpoints.v1;

[Authorize]
public sealed class OfferService(IMediator _mediator)
    : Contracts.v1.OfferService.OfferServiceBase
{
    public override async Task<ValidateOfferIdResponse> ValidateOfferId(ValidateOfferIdRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetOfferResponse> GetOffer(GetOfferRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetOfferListResponse> GetOfferList(GetOfferListRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetMortgageDetailResponse> GetMortgageDetail(GetMortgageDetailRequest request, ServerCallContext context)
       => await _mediator.Send(request);

    public override async Task<SimulateBuildingSavingsResponse> SimulateBuildingSavings(SimulateBuildingSavingsRequest request, ServerCallContext context) => 
        await _mediator.Send(request);

    public override async Task<SimulateMortgageResponse> SimulateMortgage(SimulateMortgageRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<SimulateMortgageRetentionResponse> SimulateMortgageRetention(SimulateMortgageRetentionRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<SimulateMortgageRefixationResponse> SimulateMortgageRefixation(SimulateMortgageRefixationRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<SimulateMortgageExtraPaymentResponse> SimulateMortgageExtraPayment(SimulateMortgageExtraPaymentRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetMortgageOfferFPScheduleResponse> GetMortgageOfferFPSchedule(GetMortgageOfferFPScheduleRequest request, ServerCallContext context)
      => await _mediator.Send(request);

    public override async Task<GetOfferDeveloperResponse> GetOfferDeveloper(GetOfferDeveloperRequest request, ServerCallContext context)
      => await _mediator.Send(request);

    public override async Task<Empty> UpdateOffer(UpdateOfferRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetInterestRateResponse> GetInterestRate(GetInterestRateRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<CreateResponseCodeResponse> CreateResponseCode(CreateResponseCodeRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<GetResponseCodeListResponse> GetResponseCodeList(GetResponseCodeListRequest request, ServerCallContext context)
        => await _mediator.Send(request);

    public override async Task<Empty> DeleteOfferList(DeleteOfferListRequest request, ServerCallContext context)
    {
        await _mediator.Send(request);

        return new Empty();
    }
}
