using DomainServices.DocumentOnSAService.Clients.Interfaces;
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

    public async Task<GenerateFormIdResponse> GenerateFormId(GenerateFormIdRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.GenerateFormIdAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<GetDocumentsToSignListResponse> GetDocumentsToSignList(GetDocumentsToSignListRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.GetDocumentsToSignListAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<StartSigningResponse> StartSigning(StartSigningRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.StartSigningAsync(request, cancellationToken: cancellationToken);
    }

    public async Task StopSigning(StopSigningRequest request, CancellationToken cancellationToken = default)
    {
        await _client.StopSigningAsync(request, cancellationToken: cancellationToken);
    }
}
