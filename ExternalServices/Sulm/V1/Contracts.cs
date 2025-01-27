//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#nullable enable

using System.Text.Json.Serialization;

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace ExternalServices.Sulm.V1.Contracts
{
    using System = global::System;

    

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class RegisterClientPurposeRequest
    {
        /// <summary>
        /// Id klienta.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("clientId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string ClientId { get; set; } = default!;

        /// <summary>
        /// Kód účelu.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("purposeCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string PurposeCode { get; set; } = default!;

        /// <summary>
        /// Id Kanálu.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("channelCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string ChannelCode { get; set; } = default!;

        /// <summary>
        /// Id Volajícího.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("userId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string UserId { get; set; } = default!;

        /// <summary>
        /// Typ Id volajícího.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("userIdType")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string UserIdType { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class TerminateClientPurposeRequest
    {
        /// <summary>
        /// Id klienta.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("clientId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string ClientId { get; set; } = default!;

        /// <summary>
        /// Kód účelu.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("purposeCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string PurposeCode { get; set; } = default!;

        /// <summary>
        /// Id Kanálu.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("channelCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string ChannelCode { get; set; } = default!;

        /// <summary>
        /// Id Volajícího.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("userId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string UserId { get; set; } = default!;

        /// <summary>
        /// Typ Id volajícího.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("userIdType")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string UserIdType { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class GetClientCategoryResponse
    {
        /// <summary>
        /// Kód stavu klienta.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("returnValue")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string ReturnValue { get; set; } = default!;

        /// <summary>
        /// Návratový kód
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("returnCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string ReturnCode { get; set; } = default!;

        /// <summary>
        /// Slovní popis případné chyby nebo provedené akce.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("returnText")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string ReturnText { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class GetClientActivePurposesResponse
    {

        [System.Text.Json.Serialization.JsonPropertyName("purposes")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<ActivePurpose> Purposes { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class GetClientPurposeResponse
    {

        [System.Text.Json.Serialization.JsonPropertyName("purpose")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Purpose Purpose { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Purpose
    {
        /// <summary>
        /// Účel záznamu je aktivní.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("isActive")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public bool IsActive { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ActivePurpose
    {
        /// <summary>
        /// Kód účelu - přiděluje vlastník služby SULM.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("purposeCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? PurposeCode { get; set; } = default!;

        /// <summary>
        /// Plánovaný konec platnosti (expirace) účelu.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("validTo")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset? ValidTo { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class SulmResponse
    {
        /// <summary>
        /// Návratový kód.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("returnCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string ReturnCode { get; set; } = default!;

        /// <summary>
        /// Slovní popis případné chyby nebo provedené akce.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("returnText")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string ReturnText { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// Errors as defined in https://wiki.kb.cz/confluence/display/APIM/Error+processing
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Errors
    {

        [System.Text.Json.Serialization.JsonPropertyName("errors")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object Errors1 { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Error
    {
        /// <summary>
        /// Error code
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("code")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Code { get; set; } = default!;

        /// <summary>
        /// Error type
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("type")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Type { get; set; } = default!;

        /// <summary>
        /// Error category
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("category")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Category { get; set; } = default!;

        /// <summary>
        /// Error scope
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("scope")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Scope { get; set; } = default!;

        /// <summary>
        /// Error unique identifier
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("uuid")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Uuid { get; set; } = default!;

        /// <summary>
        /// list of named parameters for current message
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("data")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object Data { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class XKBOrigSystemIdentity
    {

        [System.Text.Json.Serialization.JsonPropertyName("app")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string App { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("appComp")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public string AppComp { get; set; } = default!;

        private System.Collections.Generic.IDictionary<string, object>? _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    internal class DateFormatConverter : System.Text.Json.Serialization.JsonConverter<System.DateTimeOffset>
    {
        public override System.DateTimeOffset Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
        {
            var dateTime = reader.GetString();
            if (dateTime == null)
            {
                throw new System.Text.Json.JsonException("Unexpected JsonTokenType.Null");
            }

            return System.DateTimeOffset.Parse(dateTime);
        }

        public override void Write(System.Text.Json.Utf8JsonWriter writer, System.DateTimeOffset value, System.Text.Json.JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }


}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore  472
#pragma warning restore  114
#pragma warning restore  108
#pragma warning restore 3016
#pragma warning restore 8603