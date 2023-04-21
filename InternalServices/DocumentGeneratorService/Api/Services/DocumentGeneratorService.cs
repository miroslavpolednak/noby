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
        if (request.DocumentTypeId is 4 or 5 && request.DocumentTemplateVariantId is null)
        {
            var variantId = request.DocumentTypeId switch
            {
                4 => 2,
                5 => 6
            };

            request.DocumentTemplateVariantId = variantId;
            foreach (var part in request.Parts)
            {
                part.DocumentTemplateVariantId = variantId;
            }
        }

        return _documentManager.GenerateDocument(request);
    }
}