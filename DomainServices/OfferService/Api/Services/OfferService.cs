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

    public override async Task<SimulateBuildingSavingsResponse> SimulateBuildingSavings(SimulateBuildingSavingsRequest request, ServerCallContext context)
        => await _mediator.Send(new SimulateBuildingSavingsMediatrRequest(request));

    public override async Task<GetBuildingSavingsDataResponse> GetBuildingSavingsData(OfferInstanceIdRequest request, ServerCallContext context)
        => await _mediator.Send(new GetBuildingSavingsDataMediatrRequest(request));

    public override async Task<GetBuildingSavingsScheduleResponse> GetBuildingSavingsDepositSchedule(OfferInstanceIdRequest request, ServerCallContext context)
        => await _mediator.Send(new GetBuildingSavingsScheduleRequest(request.OfferInstanceId, ScheduleItemTypes.DepositSchedule));

    public override async Task<GetBuildingSavingsScheduleResponse> GetBuildingSavingsPaymentSchedule(OfferInstanceIdRequest request, ServerCallContext context)
        => await _mediator.Send(new GetBuildingSavingsScheduleRequest(request.OfferInstanceId, ScheduleItemTypes.PaymentSchedule));

    public override async Task<PrintBuildingSavingsOfferResponse> PrintBuildingSavingsOffer(PrintBuildingSavingsOfferRequest request, ServerCallContext context)
        => await _mediator.Send(new PrintBuildingSavingsOfferMediatrRequest(request));
}
