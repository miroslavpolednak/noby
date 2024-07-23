using DomainServices.SalesArrangementService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.SalesArrangementService.Api.Endpoints.v1;

[Authorize]
internal sealed class SalesArrangementService(IMediator _mediator)
		: Contracts.v1.SalesArrangementService.SalesArrangementServiceBase
{
	public override async Task<GetProductSalesArrangementsResponse> GetProductSalesArrangements(GetProductSalesArrangementsRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<CreateSalesArrangementResponse> CreateSalesArrangement(CreateSalesArrangementRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<SalesArrangement> GetSalesArrangement(GetSalesArrangementRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetSalesArrangementByOfferIdResponse> GetSalesArrangementByOfferId(GetSalesArrangementByOfferIdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetSalesArrangementListResponse> GetSalesArrangementList(GetSalesArrangementListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateSalesArrangementState(UpdateSalesArrangementStateRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateSalesArrangement(UpdateSalesArrangementRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateSalesArrangementParameters(UpdateSalesArrangementParametersRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkModelationToSalesArrangement(LinkModelationToSalesArrangementRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> SendToCmp(SendToCmpRequest request, ServerCallContext context)
       => await _mediator.Send(request, context.CancellationToken);

    public override async Task<ValidateSalesArrangementResponse> ValidateSalesArrangement(ValidateSalesArrangementRequest request, ServerCallContext context)
       => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateLoanAssessmentParameters(UpdateLoanAssessmentParametersRequest request, ServerCallContext context)
       => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteSalesArrangement(DeleteSalesArrangementRequest request, ServerCallContext context)
       => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetFlowSwitchesResponse> GetFlowSwitches(GetFlowSwitchesRequest request, ServerCallContext context)
       => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> SetFlowSwitches(SetFlowSwitchesRequest request, ServerCallContext context)
       => await _mediator.Send(request, context.CancellationToken);

    public override async Task<SetContractNumberResponse> SetContractNumber(SetContractNumberRequest request, ServerCallContext context) =>
        await _mediator.Send(request, context.CancellationToken);

    public override async Task<ValidateSalesArrangementIdResponse> ValidateSalesArrangementId(ValidateSalesArrangementIdRequest request, ServerCallContext context) =>
        await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdatePcpId(UpdatePcpIdRequest request, ServerCallContext context) =>
        await _mediator.Send(request, context.CancellationToken);
}
