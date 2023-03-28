using Grpc.Core;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services;

internal class DocumentGeneratorService : Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceBase
{
    private readonly PdfDocumentManager _documentManager;

    public DocumentGeneratorService(PdfDocumentManager documentManager)
    {
        _documentManager = documentManager;
    }

    public override Task<Contracts.Document> GenerateDocument(GenerateDocumentRequest request, ServerCallContext context)
    {
        //TODO: mock
        if (request.DocumentTypeId is 4 or 5 && string.IsNullOrWhiteSpace(request.DocumentTemplateVariant))
        {
            request.DocumentTemplateVariant = "B";
            foreach (var part in request.Parts)
            {
                part.DocumentTemplateVariant = "B";
            }
        }

        return _documentManager.GenerateDocument(request);
    }
}