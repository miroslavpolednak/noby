using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Grpc.Core;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services;

internal class DocumentGeneratorService : Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceBase
{
    public override Task<Document> GenerateDocument(GenerateDocumentRequest request, ServerCallContext context)
    {
        return base.GenerateDocument(request, context);
    }
}