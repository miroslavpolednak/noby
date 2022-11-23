using DomainServices.DocumentArchiveService.Api.Endpoints.UploadDocument;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentArchiveService.Contracts.v1;
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
        => await _mediator.Send(new GenerateDocumentId.GenerateDocumentIdMediatrRequest(request), context.CancellationToken);

    public override async Task<Empty> UploadDocument(UploadDocumentRequest request, ServerCallContext context)
    {
        await _mediator.Send(new UploadDocumentMediatrRequest(request), context.CancellationToken);
        return new Empty();
    }
}
