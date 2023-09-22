namespace SharedComponents.GrpcServiceBuilderHelpers;

/// <summary>
/// Nastavení pro gRPC - json transcoding
/// </summary>
public sealed class JsonTranscodingOptions
{
    /// <summary>
    /// Název projektu v OpenApi
    /// </summary>
    public string? OpenApiTitle { get; set; }

    /// <summary>
    /// Verze API v URL
    /// </summary>
    public string OpenApiVersion { get; set; } = "v1";

    /// <summary>
    /// Verze API v popisu
    /// </summary>
    public string OpenApiEndpointVersion { get; set; } = "1.0";

    internal List<string>? OpenApiXmlCommentsPaths { get; set; }

    /// <summary>
    /// Přidá cestu k souboru s XML dokumentací pro OpenApi
    /// </summary>
    public JsonTranscodingOptions AddOpenApiXmlComment(string path)
    {
        OpenApiXmlCommentsPaths ??= new List<string>();
        OpenApiXmlCommentsPaths.Add(path);

        return this;
    }

    internal JsonTranscodingOptions() { }
}
