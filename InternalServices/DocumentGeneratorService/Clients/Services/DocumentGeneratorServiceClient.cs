using CIS.Core.Results;
using CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace CIS.InternalServices.DocumentGeneratorService.Clients;

internal class DocumentGeneratorServiceClient : IDocumentGeneratorServiceClient
{
    private readonly Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient _service;

    public DocumentGeneratorServiceClient(Contracts.V1.DocumentGeneratorService.DocumentGeneratorServiceClient service)
    {
        _service = service;
    }

    public async Task<IServiceCallResult> GenerateDocument(GenerateDocumentRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GenerateDocumentAsync(request, cancellationToken: cancellationToken);

        return new SuccessfulServiceCallResult<Document>(result);
    }
}