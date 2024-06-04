namespace NOBY.Infrastructure.Swagger;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class SwaggerConfluenceLinkAttribute
    : Attribute
{
    public string ConfluenceUrl { get; init; }

    public SwaggerConfluenceLinkAttribute(string confluenceUrl)
    {
        ConfluenceUrl = confluenceUrl;
    }
}
