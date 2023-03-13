using DomainServices.DocumentArchiveService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DomainServices.DocumentArchiveService.Api.Endpoints;

[Authorize]
public class DocumentArchiveServiceGrpc : Contracts.v1.DocumentArchiveService.DocumentArchiveServiceBase
{
    private readonly IMediator _mediator;

    public DocumentArchiveServiceGrpc(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GenerateDocumentIdResponse> GenerateDocumentId(GenerateDocumentIdRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> UploadDocument(UploadDocumentRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetDocumentResponse> GetDocument(GetDocumentRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetDocumentListResponse> GetGetDocumentList(GetDocumentListRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);

    public override async Task<GetDocumentsInQueueResponse> GetDocumentsInQueue(GetDocumentsInQueueRequest request, ServerCallContext context)
     => await _mediator.Send(request, context.CancellationToken);
}
