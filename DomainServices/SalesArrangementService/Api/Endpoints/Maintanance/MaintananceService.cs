using DomainServices.SalesArrangementService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.SalesArrangementService.Api.Endpoints.Maintanance;

[Authorize]
internal sealed class MaintananceService(IMediator _mediator)
		: Contracts.MaintananceService.MaintananceServiceBase
{
    public override async Task<Empty> CancelNotFinishedExtraPayments(CancelNotFinishedExtraPaymentsRequest request, ServerCallContext context)
      => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetCancelCaseJobIdsResponse> GetCancelCaseJobIds(GetCancelCaseJobIdsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetCancelServiceSalesArrangementsIdsResponse> GetCancelServiceSalesArrangementsIds(GetCancelServiceSalesArrangementsIdsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetOfferGuaranteeDateToCheckResponse> GetOfferGuaranteeDateToCheck(GetOfferGuaranteeDateToCheckRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);
}
