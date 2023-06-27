namespace NOBY.Infrastructure.Swagger;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class SwaggerEaDiagramAttribute
    : Attribute
{
    public string DiagramUrl { get; init; }

    public SwaggerEaDiagramAttribute(string diagramUrl)
    {
        DiagramUrl = diagramUrl;
    }
}
