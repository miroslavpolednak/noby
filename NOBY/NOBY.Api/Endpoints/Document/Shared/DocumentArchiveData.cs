namespace NOBY.Api.Endpoints.Document.Shared;

internal class DocumentArchiveData
{
    public required string DocumentId { get; init; }

    public required long CaseId { get; init; }

    public required int UserId { get; set; }

    public required ReadOnlyMemory<byte> DocumentData { get; init; }

    public required string FileName { get; init; }

    public required int DocumentTypeId { get; init; }

    public string? ContractNumber { get; init; }
}