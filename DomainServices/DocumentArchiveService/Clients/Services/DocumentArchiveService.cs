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
}
