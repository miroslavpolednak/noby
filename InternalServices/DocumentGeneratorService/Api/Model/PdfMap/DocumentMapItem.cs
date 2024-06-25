namespace CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;

public record DocumentMapItem
{
    public required FieldInfo Field { get; init; }

    public bool HasChildFields => ChildFields?.Any() ?? false;

    public List<FieldInfo>? ChildFields { get; init; } = [];
}