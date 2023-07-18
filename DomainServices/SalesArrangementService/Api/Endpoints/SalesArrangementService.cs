using DomainServices.SalesArrangementService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.SalesArrangementService.Api.Endpoints;

[Authorize]
internal sealed class SalesArrangementService : Contracts.v1.SalesArrangementService.SalesArrangementServiceBase
{
    private readonly IMediator _mediator;

    public SalesArrangementService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<GetProductSalesArrangementResponse> GetProductSalesArrangement(GetProductSalesArrangementRequest request, ServerCallContext context)
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

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateOfferDocumentId(UpdateOfferDocumentIdRequest request, ServerCallContext context)
       => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetFlowSwitchesResponse> GetFlowSwitches(GetFlowSwitchesRequest request, ServerCallContext context)
       => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> SetFlowSwitches(SetFlowSwitchesRequest request, ServerCallContext context)
       => await _mediator.Send(request, context.CancellationToken);

    public override async Task<SetContractNumberResponse> SetContractNumber(SetContractNumberRequest request, ServerCallContext context) => 
        await _mediator.Send(request, context.CancellationToken);
}
