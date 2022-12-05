using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Grpc.Core;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services;

internal class DocumentGeneratorService : Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceBase
{
    private readonly PdfDocumentManager _documentManager;

    public DocumentGeneratorService(PdfDocumentManager documentManager)
    {
        _documentManager = documentManager;
    }

    public override Task<Document> GenerateDocument(GenerateDocumentRequest request, ServerCallContext context)
    {
        return _documentManager.GenerateDocument(request);
    }
}