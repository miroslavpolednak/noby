namespace CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;

public record FieldCoordinates
{
    public required float X { get; init; }

    public required float Y { get; init; }

    public required float Width { get; init; }

    public required float Height { get; init; }

}