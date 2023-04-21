namespace DomainServices.CustomerService.ExternalServices.Common.Dto;

internal sealed class Error
{
    /// <summary>
    /// Http status code
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("httpStatusCode")]
    public int HttpStatusCode { get; set; }

    /// <summary>
    /// Error category
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("category")]
    public int Category { get; set; }

    /// <summary>
    /// Error code
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("code")]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Code { get; set; }

    /// <summary>
    /// Error message
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("message")]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Message { get; set; }

    /// <summary>
    /// Error uuid
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("uuid")]
    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    public string Uuid { get; set; }

    /// <summary>
    /// Error detail
    /// </summary>

    [System.Text.Json.Serialization.JsonPropertyName("detail")]

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public object Detail { get; set; }

    private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

    [System.Text.Json.Serialization.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
        set { _additionalProperties = value; }
    }

}
