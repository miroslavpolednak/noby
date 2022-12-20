using CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace CIS.InternalServices.DocumentGeneratorService.Clients.Services;

internal class DocumentGeneratorServiceClient : IDocumentGeneratorServiceClient
{
    private readonly Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient _service;

    public DocumentGeneratorServiceClient(Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient service)
    {
        _service = service;
    }

    public Task<Document> GenerateDocument(GenerateDocumentRequest request, CancellationToken cancellationToken = default)
    {
        return _service.GenerateDocumentAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }
}