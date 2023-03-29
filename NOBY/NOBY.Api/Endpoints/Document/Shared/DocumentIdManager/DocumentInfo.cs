namespace NOBY.Api.Endpoints.Document.Shared.DocumentIdManager;

internal class DocumentInfo
{
    public required string DocumentId { get; init; }

    public string? ContractNumber { get; init; }
}