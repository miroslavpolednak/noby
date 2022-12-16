using CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace CIS.InternalServices.DocumentGeneratorService.Clients;

public interface IDocumentGeneratorServiceClient
{
    Task<Document> GenerateDocument(GenerateDocumentRequest request, CancellationToken cancellationToken = default);
}