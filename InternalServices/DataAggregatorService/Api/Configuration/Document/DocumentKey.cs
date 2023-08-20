namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

public record DocumentKey(int TypeId, DocumentVersionData VersionData)
{
    public object CreateSqlParams()
    {
        return new
        {
            DocumentId = TypeId,
            DocumentVersion = VersionData.VersionName,
            DocumentVariant = VersionData.VariantName ?? string.Empty
        };
    }
}

public record DocumentVersionData
{
    public required int VersionId { get; init; }

    public required string VersionName { get; init; }

    public int? VariantId { get; init; }

    public string? VariantName { get; init; }
}