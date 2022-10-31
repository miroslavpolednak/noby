namespace DomainServices.DocumentArchiveService.Api.Endpoints;

[Authorize]
public class DocumentArchiveServiceGrpc
    : Contracts.IDocumentArchiveService
{
    private readonly IMediator _mediator;

    public DocumentArchiveServiceGrpc(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<Contracts.GenerateDocumentIdResponse> GenerateDocumentId(Contracts.GenerateDocumentIdRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(new GenerateDocumentId.GenerateDocumentIdMediatrRequest(request), cancellationToken);
}
