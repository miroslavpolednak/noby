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

namespace DomainServices.CodebookService.ExternalServices.AcvEnumService.V1.Contracts
{
    using System = global::System;

    

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class EnumItemDTO
    {

        [System.Text.Json.Serialization.JsonPropertyName("code")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Code { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("text")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Text { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("arch")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public bool Arch { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ExceptionResultModel
    {

        [System.Text.Json.Serialization.JsonPropertyName("code")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Code { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("detail")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Detail { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("message")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Message { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("uuid")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Guid Uuid { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("originalCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? OriginalCode { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ParameterValidationErrorModel
    {

        [System.Text.Json.Serialization.JsonPropertyName("attribute")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Attribute { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("message")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Message { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("rootObjectName")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? RootObjectName { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("invalidValue")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object? InvalidValue { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ValidationErrorModel
    {

        [System.Text.Json.Serialization.JsonPropertyName("code")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Code { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("message")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Message { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("uuid")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Guid Uuid { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("originalErrorCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string? OriginalErrorCode { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("validationDetail")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<ParameterValidationErrorModel>? ValidationDetail { get; set; } = default!;

    }


}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore  472
#pragma warning restore  114
#pragma warning restore  108
#pragma warning restore 3016
#pragma warning restore 8603