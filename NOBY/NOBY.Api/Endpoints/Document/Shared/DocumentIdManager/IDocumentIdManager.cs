namespace NOBY.Api.Endpoints.Document.Shared.DocumentIdManager;

internal interface IDocumentIdManager<in TEntityId>
{
    Task<string> LoadDocumentId(TEntityId entityId, CancellationToken cancellationToken);

    Task UpdateDocumentId(TEntityId entityId, string documentId, CancellationToken cancellationToken);
}