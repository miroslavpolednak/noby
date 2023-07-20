using DomainServices.RealEstateValuationService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RealEstateValuationService.Api.Endpoints;

[Authorize]
internal sealed class RealEstateValuationService
    : Contracts.v1.RealEstateValuationService.RealEstateValuationServiceBase
{
    public override async Task<CreateRealEstateValuationResponse> CreateRealEstateValuation(CreateRealEstateValuationRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> DeleteRealEstateValuation(DeleteRealEstateValuationRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetRealEstateValuationListResponse> GetRealEstateValuationList(GetRealEstateValuationListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> PatchDeveloperOnRealEstateValuation(PatchDeveloperOnRealEstateValuationRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<RealEstateValuationDetail> GetRealEstateValuationDetail(GetRealEstateValuationDetailRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> UpdateRealEstateValuationDetail(UpdateRealEstateValuationDetailRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> SetACVRealEstateTypeByRealEstateValuation(SetACVRealEstateTypeByRealEstateValuationRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<AddDeedOfOwnershipDocumentResponse> AddDeedOfOwnershipDocument(AddDeedOfOwnershipDocumentRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> DeleteDeedOfOwnershipDocument(DeleteDeedOfOwnershipDocumentRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<CreateRealEstateValuationAttachmentResponse> CreateRealEstateValuationAttachment(CreateRealEstateValuationAttachmentRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> DeleteRealEstateValuationAttachment(DeleteRealEstateValuationAttachmentRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<ValidateRealEstateValuationIdResponse> ValidateRealEstateValuationId(ValidateRealEstateValuationIdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    private readonly IMediator _mediator;
    public RealEstateValuationService(IMediator mediator)
        => _mediator = mediator;
}
