using CIS.Core.Exceptions;
using Grpc.Core;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services;

internal class DocumentGeneratorService : Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceBase
{
    private readonly PdfDocumentManager _documentManager;

    public DocumentGeneratorService(PdfDocumentManager documentManager)
    {
        _documentManager = documentManager;
    }

    public override async Task<Contracts.Document> GenerateDocument(GenerateDocumentRequest request, ServerCallContext context)
    {
        try
        {
            return await _documentManager.GenerateDocument(request);
        }
        catch (Exception e)
        {
            throw new CisException(0, e.StackTrace!);
        }
    }
}