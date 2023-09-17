namespace SharedComponents.GrpcServiceBuilderHelpers;

/// <summary>
/// Nastavení pro gRPC - json transcoding
/// </summary>
public sealed class JsonTranscodingOptions
{
    /// <summary>
    /// Název projektu v OpenApi
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Verze API v URL
    /// </summary>
    public string SwaggerApiVersion { get; set; } = "v1";

    /// <summary>
    /// Verze API v popisu
    /// </summary>
    public string SwaggerEndpointVersion { get; set; } = "1.0";

    internal List<string>? XmlCommentsPaths { get; set; }

    /// <summary>
    /// Přidá cestu k souboru s XML dokumentací pro OpenApi
    /// </summary>
    public JsonTranscodingOptions AddXmlCommentsPath(string path)
    {
        XmlCommentsPaths ??= new List<string>();
        XmlCommentsPaths.Add(path);

        return this;
    }

    internal JsonTranscodingOptions() { }
}
