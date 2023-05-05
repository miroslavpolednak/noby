namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

public record DocumentVersionData
{
    public required int VersionId { get; init; }

    public required string VersionName { get; init; }

    public int? VariantId { get; init; }

    public string? VariantName { get; init; }
}