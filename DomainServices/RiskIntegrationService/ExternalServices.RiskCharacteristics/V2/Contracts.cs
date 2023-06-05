//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 612 // Disable "CS0612 '...' is obsolete"
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V2.Contracts
{
    using System = global::System;

    

    /// <summary>
    /// Amount
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Amount
    {
        /// <summary>
        /// Currency code of the Amount (ISO 4217), e.g. 'CZK', 'EUR'
        /// <br/>NotNull
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("currencyCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Value of the Amount
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("value")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal Value { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// HouseholdCreditLiabilitiesSummaryOutHomeCompany.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CreditLiabilitiesSummary
    {

        [System.Text.Json.Serialization.JsonPropertyName("amount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount Amount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("amountConsolidated")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount AmountConsolidated { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("productClusterCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public CreditLiabilitiesSummaryType? ProductClusterCode { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// CREDIT LIABILITIES PRODUCT CLUSTER
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum CreditLiabilitiesSummaryType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"CC")]
        CC = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"AD")]
        AD = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"CL")]
        CL = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"ML")]
        ML = 3,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DSTICalculation
    {
        /// <summary>
        /// DSTI
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("dsti")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public decimal Dsti { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DSTICalculationArguments
    {

        [System.Text.Json.Serialization.JsonPropertyName("itChannel")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public ItChannel ItChannel { get; set; }

        /// <summary>
        /// loanApplicationHousehold
        /// <br/>NotNull
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("loanApplicationHousehold")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<DSTILoanApplicationHousehold> LoanApplicationHousehold { get; set; } = new System.Collections.ObjectModel.Collection<DSTILoanApplicationHousehold>();

        [System.Text.Json.Serialization.JsonPropertyName("loanApplicationProduct")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public DSTILoanApplicationProductInput LoanApplicationProduct { get; set; } = new DSTILoanApplicationProductInput();

        [System.Text.Json.Serialization.JsonPropertyName("resourceProcessId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^urn:ri:(\w+)\.(\w+)\.(\w+)\.([\w+-.]*)(~(\w+))?")]
        public string ResourceProcessId { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// DSTICreditLiabilitiesSummary.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DSTICreditLiabilitiesSummary
    {

        [System.Text.Json.Serialization.JsonPropertyName("amount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount Amount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("amountConsolidated")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount AmountConsolidated { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("productClusterCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public DSTICreditLiabilitiesSummaryType? ProductClusterCode { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// CreditLiabilitySummaryType
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum DSTICreditLiabilitiesSummaryType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"CC")]
        CC = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"AD")]
        AD = 1,

    }

    /// <summary>
    /// loanApplicationHousehold
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DSTILoanApplicationHousehold
    {
        /// <summary>
        /// Výše závazků v KB
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("creditLiabilitiesSummaryHomeCompany")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<DSTICreditLiabilitiesSummary> CreditLiabilitiesSummaryHomeCompany { get; set; }

        /// <summary>
        /// Výše závazků mimo KB
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("creditLiabilitiesSummaryOutHomeCompany")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<DSTICreditLiabilitiesSummary> CreditLiabilitiesSummaryOutHomeCompany { get; set; }

        /// <summary>
        /// loanApplication Counterparty
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("loanApplicationCounterparty")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<LoanApplicationCounterparty> LoanApplicationCounterparty { get; set; }

        /// <summary>
        /// Výše splátek úvěrových produktů v KB
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("loanInstallmentsSummaryHomeCompany")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<LoanInstallmentsSummary> LoanInstallmentsSummaryHomeCompany { get; set; }

        /// <summary>
        /// Výše splátek úvěrových produků mimo KB
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("loanInstallmentsSummaryOutHomeCompany")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<LoanInstallmentsSummary> LoanInstallmentsSummaryOutHomeCompany { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// loanApplicationProduct
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DSTILoanApplicationProductInput
    {

        [System.Text.Json.Serialization.JsonPropertyName("amountRequired")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount AmountRequired { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("annuity")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount Annuity { get; set; }

        /// <summary>
        /// Kód produktového shluku
        /// <br/>NotNull
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("productClusterCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string ProductClusterCode { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DTICalculation
    {
        /// <summary>
        /// DTI
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("dti")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public decimal Dti { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DTICalculationArguments
    {

        [System.Text.Json.Serialization.JsonPropertyName("itChannel")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public ItChannel ItChannel { get; set; }

        /// <summary>
        /// loanApplicationHousehold
        /// <br/>NotNull
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("loanApplicationHousehold")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<DTILoanApplicationHousehold> LoanApplicationHousehold { get; set; } = new System.Collections.ObjectModel.Collection<DTILoanApplicationHousehold>();

        [System.Text.Json.Serialization.JsonPropertyName("loanApplicationProduct")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public DTILoanApplicationProductInput LoanApplicationProduct { get; set; } = new DTILoanApplicationProductInput();

        [System.Text.Json.Serialization.JsonPropertyName("resourceProcessId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^urn:ri:(\w+)\.(\w+)\.(\w+)\.([\w+-.]*)(~(\w+))?")]
        public string ResourceProcessId { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// loanApplicationHousehold
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DTILoanApplicationHousehold
    {
        /// <summary>
        /// Výše závazků v KB
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("creditLiabilitiesSummaryHomeCompany")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<CreditLiabilitiesSummary> CreditLiabilitiesSummaryHomeCompany { get; set; }

        /// <summary>
        /// Výše závazků mimo KB
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("creditLiabilitiesSummaryOutHomeCompany")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<CreditLiabilitiesSummary> CreditLiabilitiesSummaryOutHomeCompany { get; set; }

        /// <summary>
        /// loanApplication Counterparty
        /// <br/>
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("loanApplicationCounterparty")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public System.Collections.Generic.ICollection<LoanApplicationCounterparty> LoanApplicationCounterparty { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// loanApplicationProduct
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class DTILoanApplicationProductInput
    {

        [System.Text.Json.Serialization.JsonPropertyName("amountRequired")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.ComponentModel.DataAnnotations.Required]
        public Amount? AmountRequired { get; set; } = new Amount();

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// The unified error model
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ErrorModel
    {
        /// <summary>
        /// usually unused and replaced by the error codes categorization
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("category")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Obsolete]
        public int Category { get; set; }

        /// <summary>
        /// code of the error that occured, with or without an hirarchical categorization
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("code")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Code { get; set; }

        /// <summary>
        /// an optional error detail with a custom schema discriminated by the type-discriminator
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("detail")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public object Detail { get; set; }

        /// <summary>
        /// copy of the http-status-code
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("httpStatusCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// plain-text message description related to the given error code and intended for better issue-solving only
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("message")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Message { get; set; }

        /// <summary>
        /// unique error ID
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("uuid")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public string Uuid { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// IT aplikace volající službu (PF...)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum ItChannel
    {

        [System.Runtime.Serialization.EnumMember(Value = @"AON")]
        AON = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"DCS")]
        DCS = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"NOBY")]
        NOBY = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"PF")]
        PF = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"PFO")]
        PFO = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"STARBUILD")]
        STARBUILD = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"NCL")]
        NCL = 6,

    }

    /// <summary>
    /// loanApplication Counterparty
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class LoanApplicationCounterparty
    {

        [System.Text.Json.Serialization.JsonPropertyName("customerId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^urn:ri:(\w+)\.(\w+)\.(\w+)\.([\w+-.]*)(~(\w+))?")]
        public string CustomerId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("entrepreneurAnnualIncomeAmount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount EntrepreneurAnnualIncomeAmount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("monthlyEmploymentIncomeSumAmount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount MonthlyEmploymentIncomeSumAmount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("monthlyOtherIncomeAmount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount MonthlyOtherIncomeAmount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("monthlyRentIncomeSumAmount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount MonthlyRentIncomeSumAmount { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// householdInstallmentsSummaryOutHomeCompany
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class LoanInstallmentsSummary
    {

        [System.Text.Json.Serialization.JsonPropertyName("amount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount Amount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("amountConsolidated")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        public Amount AmountConsolidated { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("productClusterCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]   
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public LoanInstallmentsSummaryType? ProductClusterCode { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// INSTALLMENTS PRODUCT CLUSTER
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.18.5.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum LoanInstallmentsSummaryType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"CL")]
        CL = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"ML")]
        ML = 1,

    }


}

#pragma warning restore  108
#pragma warning restore  114
#pragma warning restore  472
#pragma warning restore  612
#pragma warning restore 1573
#pragma warning restore 1591
#pragma warning restore 8073
#pragma warning restore 3016
#pragma warning restore 8603