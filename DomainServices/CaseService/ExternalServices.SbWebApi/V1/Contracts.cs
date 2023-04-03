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

namespace DomainServices.CaseService.ExternalServices.SbWebApi.V1.Contracts
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class WFS_Header
    {

        [JsonPropertyName("system")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string? System { get; set; } = default!;

        [JsonPropertyName("login")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Login { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class WFS_Event_Response
    {

        [JsonPropertyName("request_id")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int? Request_id { get; set; } = default!;

        [JsonPropertyName("result")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public CommonResult? Result { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class CommonResult
    {

        [JsonPropertyName("return_val")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int? Return_val { get; set; } = default!;

        [JsonPropertyName("return_text")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Return_text { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class WFS_Request_CaseStateChanged
    {

        [JsonPropertyName("header")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public WFS_Header? Header { get; set; } = default!;

        [JsonPropertyName("message")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public WFS_Event_CaseStateChanged? Message { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.0.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class WFS_Event_CaseStateChanged
    {

        [JsonPropertyName("case_id")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public long? Case_id { get; set; } = default!;

        [JsonPropertyName("uver_id")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public long? Uver_id { get; set; } = default!;

        [JsonPropertyName("contract_no")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Contract_no { get; set; } = default!;

        [JsonPropertyName("loan_no")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Loan_no { get; set; } = default!;

        [JsonPropertyName("jmeno_prijmeni")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Jmeno_prijmeni { get; set; } = default!;

        [JsonPropertyName("case_state")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Case_state { get; set; } = default!;

        [JsonPropertyName("product_type")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int? Product_type { get; set; } = default!;

        [JsonPropertyName("owner_cpm")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Owner_cpm { get; set; } = default!;

        [JsonPropertyName("owner_icp")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Owner_icp { get; set; } = default!;

        [JsonPropertyName("mandant")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int? Mandant { get; set; } = default!;

        [JsonPropertyName("client_benefits")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public int? Client_benefits { get; set; } = default!;

        [JsonPropertyName("risk_business_case_id")]

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]   
        public string? Risk_business_case_id { get; set; } = default!;

    }

}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore  472
#pragma warning restore  114
#pragma warning restore  108
#pragma warning restore 3016
#pragma warning restore 8603