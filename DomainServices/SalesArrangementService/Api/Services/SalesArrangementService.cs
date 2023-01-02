using DomainServices.SalesArrangementService.Contracts;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.SalesArrangementService.Api.Services;

[Authorize]
internal class SalesArrangementService : Contracts.v1.SalesArrangementService.SalesArrangementServiceBase
{
    private readonly IMediator _mediator;

    public SalesArrangementService(IMediator mediator)
        => _mediator = mediator;

    public override async Task<CreateSalesArrangementResponse> CreateSalesArrangement(CreateSalesArrangementRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.CreateSalesArrangementMediatrRequest(request), context.CancellationToken);

    public override async Task<SalesArrangement> GetSalesArrangement(SalesArrangementIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetSalesArrangementMediatrRequest(request), context.CancellationToken);
    
    public override async Task<GetSalesArrangementByOfferIdResponse> GetSalesArrangementByOfferId(GetSalesArrangementByOfferIdRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetSalesArrangementByOfferIdMediatrRequest(request), context.CancellationToken);

    public override async Task<GetSalesArrangementListResponse> GetSalesArrangementList(GetSalesArrangementListRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.GetSalesArrangementListMediatrRequest(request.CaseId), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateSalesArrangementState(UpdateSalesArrangementStateRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateSalesArrangementStateMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateSalesArrangement(UpdateSalesArrangementRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateSalesArrangementMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateSalesArrangementParameters(UpdateSalesArrangementParametersRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.UpdateSalesArrangementParametersMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> LinkModelationToSalesArrangement(LinkModelationToSalesArrangementRequest request, ServerCallContext context)
        => await _mediator.Send(new Dto.LinkModelationToSalesArrangementMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> SendToCmp(SalesArrangementIdRequest request, ServerCallContext context)
       => await _mediator.Send(new Dto.SendToCmpMediatrRequest(request), context.CancellationToken);

    public override async Task<ValidateSalesArrangementResponse> ValidateSalesArrangement(SalesArrangementIdRequest request, ServerCallContext context)
       => await _mediator.Send(new Dto.ValidateSalesArrangementMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateLoanAssessmentParameters(UpdateLoanAssessmentParametersRequest request, ServerCallContext context)
       => await _mediator.Send(new Dto.UpdateLoanAssessmentParametersMediatrRequest(request), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> DeleteSalesArrangement(DeleteSalesArrangementRequest request, ServerCallContext context)
       => await _mediator.Send(new Dto.DeleteSalesArrangementMediatrRequest(request.SalesArrangementId, request.HardDelete), context.CancellationToken);

    public override async Task<Google.Protobuf.WellKnownTypes.Empty> UpdateOfferDocumentId(UpdateOfferDocumentIdRequest request, ServerCallContext context)
       => await _mediator.Send(new Dto.UpdateOfferDocumentIdMediatrRequest(request.SalesArrangementId, request.OfferDocumentId), context.CancellationToken);
}
