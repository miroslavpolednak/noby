using DomainServices.DocumentOnSAService.Contracts;
using static DomainServices.DocumentOnSAService.Contracts.v1.DocumentOnSAService;

namespace DomainServices.DocumentOnSAService.Clients.Services;
public class DocumentOnSAService : IDocumentOnSAServiceClient
{
    private readonly DocumentOnSAServiceClient _client;

    public DocumentOnSAService(DocumentOnSAServiceClient client)
    {
        _client = client;
    }

    public async Task<string> GenerateFormId(GenerateFormIdRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _client.GenerateFormIdAsync(request, cancellationToken: cancellationToken);
        return result.FormId;
    }

    public async Task<GetDocumentsToSignListResponse> GetDocumentsToSignList(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        return await _client.GetDocumentsToSignListAsync(new GetDocumentsToSignListRequest
        {
            SalesArrangementId = salesArrangementId
        }, cancellationToken: cancellationToken);
    }

    public async Task<StartSigningResponse> StartSigning(StartSigningRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.StartSigningAsync(request, cancellationToken: cancellationToken);
    }

    public async Task StopSigning(int documentOnSAId, CancellationToken cancellationToken = default)
    {
        await _client.StopSigningAsync(new StopSigningRequest
        {
            DocumentOnSAId = documentOnSAId
        }, cancellationToken: cancellationToken);
    }

    public async Task<GetDocumentOnSADataResponse> GetDocumentOnSAData(int documentOnSAId, CancellationToken cancellationToken = default)
    {
        return await _client.GetDocumentOnSADataAsync(new() { DocumentOnSAId = documentOnSAId }, cancellationToken: cancellationToken);
    }

    public async Task SignDocumentManually(int documentOnSAId, CancellationToken cancellationToken = default)
    {
        await _client.SignDocumentManuallyAsync(new() { DocumentOnSAId = documentOnSAId }, cancellationToken: cancellationToken);
    }
}
