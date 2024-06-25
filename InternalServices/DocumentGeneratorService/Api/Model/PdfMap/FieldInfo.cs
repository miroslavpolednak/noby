namespace CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;

public record FieldInfo
{
    public required string Name { get; init; }

    public required int PageNumber { get; init; }

    public required string FontName { get; init; }

    public required float FontSize { get; init; }

    public required FieldCoordinates Coordinates { get; init; }
}