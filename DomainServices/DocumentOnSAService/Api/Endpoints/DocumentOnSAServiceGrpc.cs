using DomainServices.DocumentOnSAService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DomainServices.DocumentOnSAService.Api.Endpoints;

internal sealed class DocumentOnSAServiceGrpc : Contracts.v1.DocumentOnSAService.DocumentOnSAServiceBase
{
    private readonly IMediator _mediator;

    public DocumentOnSAServiceGrpc(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GenerateFormIdResponse> GenerateFormId(GenerateFormIdRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<StartSigningResponse> StartSigning(StartSigningRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> StopSigning(StopSigningRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetDocumentsToSignListResponse> GetDocumentsToSignList(GetDocumentsToSignListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetDocumentOnSADataResponse> GetDocumentOnSAData(GetDocumentOnSADataRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> SignDocumentManually(SignDocumentManuallyRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

}
