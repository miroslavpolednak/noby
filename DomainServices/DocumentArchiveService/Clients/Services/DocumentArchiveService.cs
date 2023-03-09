using DomainServices.DocumentArchiveService.Contracts;
using static DomainServices.DocumentArchiveService.Contracts.v1.DocumentArchiveService;

namespace DomainServices.DocumentArchiveService.Clients.Services;

public class DocumentArchiveService
    : IDocumentArchiveServiceClient
{
    private readonly DocumentArchiveServiceClient _service;

    public DocumentArchiveService(
         DocumentArchiveServiceClient service)
    {
        _service = service;
    }

    public async Task<string> GenerateDocumentId(GenerateDocumentIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.GenerateDocumentIdAsync(request, cancellationToken: cancellationToken);
        return result.DocumentId!;
    }

    public async Task<GetDocumentResponse> GetDocument(GetDocumentRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.GetDocumentAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<GetDocumentListResponse> GetDocumentList(GetDocumentListRequest request, CancellationToken cancellationToken = default)
    {
        return await _service.GetGetDocumentListAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<GetDocumentsInQueueResponse> GetDocumentsInQueue(GetDocumentsInQueueRequest request, CancellationToken cancellationToken = default)
    {
       return await _service.GetDocumentsInQueueAsync(request, cancellationToken: cancellationToken);
    }

    public async Task UploadDocument(UploadDocumentRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UploadDocumentAsync(request, cancellationToken: cancellationToken);
    }
}
