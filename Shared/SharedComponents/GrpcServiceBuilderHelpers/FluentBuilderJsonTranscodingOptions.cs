namespace SharedComponents.GrpcServiceBuilderHelpers;

/// <summary>
/// Nastavení pro gRPC - json transcoding
/// </summary>
public sealed class FluentBuilderJsonTranscodingOptions
{
    /// <summary>
    /// Pokud ma být Transcoding zapnut, nastavit na TRUE
    /// </summary>
    public bool Enabled { get; set; }

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

    /// <summary>
    /// Gets or sets a value indicating whether to write enum values as integers. Default is true.
    /// </summary>
    public bool WriteEnumsAsIntegers { get; set;} = true;

    internal List<string>? OpenApiXmlCommentsPaths { get; set; }

    /// <summary>
    /// Přidá cestu k souboru s XML dokumentací pro OpenApi
    /// </summary>
    public FluentBuilderJsonTranscodingOptions AddOpenApiXmlComment(string path)
    {
        OpenApiXmlCommentsPaths ??= new List<string>();
        OpenApiXmlCommentsPaths.Add(path);

        return this;
    }

    /// <summary>
    /// Přidá cestu k souboru z AppContext.BaseDirectory s XML dokumentací pro OpenApi
    /// </summary>
    public FluentBuilderJsonTranscodingOptions AddOpenApiXmlCommentFromBaseDirectory(string filename)
    {
        OpenApiXmlCommentsPaths ??= new List<string>();
        OpenApiXmlCommentsPaths.Add(Path.Combine(AppContext.BaseDirectory, filename));

        return this;
    }

    internal FluentBuilderJsonTranscodingOptions() { }
}
