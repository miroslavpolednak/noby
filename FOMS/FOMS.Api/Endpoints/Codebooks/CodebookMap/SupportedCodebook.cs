namespace FOMS.Api.Endpoints.Codebooks.CodebookMap;

public record SupportedCodebook
{
    public string Name { get; init; } = null!;

    public string Type { get; init; } = null!;
}