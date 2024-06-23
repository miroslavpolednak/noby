namespace CIS.InternalServices.DocumentGeneratorService.Api.Model;

public record PdfTemplate
{
    public required string Name { get; init; }

    public required string Version { get; init; }

    public required string Variant { get; init; }

    public required PdfDocument PdfDocument { get; init; }
}