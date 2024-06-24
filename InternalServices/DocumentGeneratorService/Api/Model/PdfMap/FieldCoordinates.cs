namespace CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;

public record FieldCoordinates
{
    public const float Offset = 2f;

    public required float X { get; init; }

    public required float Y { get; init; }

    public required float Width { get; init; }

    public required float Height { get; init; }

    public float XWithOffset() => X + Offset;

    public float YWithOffset() => Y + Offset;

    public float WidthWithOffset() => Width - Offset;

    public float HeightWithOffset() => Height - Offset;
}