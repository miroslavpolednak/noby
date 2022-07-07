//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts
{
    using System = global::System;

    

    /// <summary>
    /// Parametry potřebné pro výpočet Bonity
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class CreditWorthinessCalculation
    {
        /// <summary>
        /// maximální disponibilní splátka
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("installmentLimit")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public long InstallmentLimit { get; set; }

        /// <summary>
        /// maximální výše úvěru
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("maxAmount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public long MaxAmount { get; set; }

        /// <summary>
        /// Zbývá na živobytí s ANNUITY
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("remainsLivingAnnuity")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public long? RemainsLivingAnnuity { get; set; }

        /// <summary>
        /// Zbývá na živobytí s disp. Splátkou
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("remainsLivingInst")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public long? RemainsLivingInst { get; set; }

        /// <summary>
        /// ResultReson
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("resultReason")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public ResultReason ResultReason { get; set; } = new ResultReason();

    }

    /// <summary>
    /// Důvod(y) nespočetní výseldků bonity.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class ResultReason
    {
        /// <summary>
        /// kód důvodu nespočtení výsledku
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("code")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public string Code { get; set; }

        /// <summary>
        /// popis důvodu nespočtení výsledku
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("description")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public string Description { get; set; }

    }

    /// <summary>
    /// HouseholdCreditLiabilitiesSummaryOutHomeCompany.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class CreditLiabilitiesSummary
    {
        /// <summary>
        /// Product group.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("productGroup")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public CreditLiabilitiesSummaryProductGroup ProductGroup { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal Amount { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amountConsolidated")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal AmountConsolidated { get; set; }

    }

    /// <summary>
    /// HouseholdCreditLiabilitiesSummaryHomeCompany.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class CreditLiabilitiesSummaryHomeCompany
    {
        /// <summary>
        /// Product group.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("productGroup")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public CreditLiabilitiesSummaryHomeCompanyProductGroup ProductGroup { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal Amount { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amountConsolidated")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal AmountConsolidated { get; set; }

    }

    /// <summary>
    /// Parametry potřebné pro výpočet Bonity
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class CreditWorthinessCalculationArguments
    {

        [System.Text.Json.Serialization.JsonPropertyName("id")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public ResourceIdentifier Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("resourceProcessId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public ResourceIdentifier ResourceProcessId { get; set; }

        /// <summary>
        /// Identifikátor volající aplikace.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("itChannel")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public CreditWorthinessCalculationArgumentsItChannel ItChannel { get; set; }

        /// <summary>
        /// Identifikátor žádosti z pohledu Risku, nepovinné.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("riskBusinessCaseId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public long RiskBusinessCaseId { get; set; }

        /// <summary>
        /// Informace o delaerovi, který službu volá
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("loanApplicationDealer")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public Dealer LoanApplicationDealer { get; set; }

        /// <summary>
        /// Informace o interním zaměstanci, který službu volá
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("kbGroupPerson")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public Person KbGroupPerson { get; set; }

        /// <summary>
        /// Identifikátor žádosti z pohledu Risku, nepovinné.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("loanApplicationProduct")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public LoanApplicationProduct LoanApplicationProduct { get; set; } = new LoanApplicationProduct();

        /// <summary>
        /// Domácnosti.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("households")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<LoanApplicationHousehold> Households { get; set; } = new System.Collections.ObjectModel.Collection<LoanApplicationHousehold>();

    }

    /// <summary>
    /// Dealer
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class Dealer
    {
        /// <summary>
        /// Identifikátor dealera
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public ResourceIdentifier Id { get; set; } = new ResourceIdentifier();

        /// <summary>
        /// Identifikátor zprostředkovateské společnosti
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("companyId")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public ResourceIdentifier CompanyId { get; set; } = new ResourceIdentifier();

    }

    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class ExpensesSummary
    {
        /// <summary>
        /// Kategorie.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("category")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public ExpensesSummaryCategory Category { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal Amount { get; set; }

    }

    /// <summary>
    /// HouseholdInstallmentsSummaryHomeCompany.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class InstallmentsSummaryHomeCompany
    {
        /// <summary>
        /// Product group.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("productGroup")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public InstallmentsSummaryHomeCompanyProductGroup ProductGroup { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal Amount { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amountConsolidated")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal AmountConsolidated { get; set; }

    }

    /// <summary>
    /// HouseholdInstallmentsSummaryOutHomeCompany.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class InstallmentsSummaryOutHomeCompany
    {
        /// <summary>
        /// Product group.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("productGroup")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public InstallmentsSummaryOutHomeCompanyProductGroup ProductGroup { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal Amount { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amountConsolidated")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal AmountConsolidated { get; set; }

    }

    /// <summary>
    /// JobPost
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class JobPost
    {
        /// <summary>
        /// Kód pracovní pozice přihlášeného uživatele/schvalovatele
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Id { get; set; }

    }

    /// <summary>
    /// Protistrana žádosti o půjčku.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class LoanApplicationCounterParty
    {
        /// <summary>
        /// Identifikátor klienta (např. v případě KB klienta IDDI)
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public ResourceIdentifier Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("loanApplicationIncome")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<LoanApplicationIncome> LoanApplicationIncome { get; set; } = new System.Collections.ObjectModel.Collection<LoanApplicationIncome>();

        /// <summary>
        /// současný rodinný stav
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("isPartner")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public int IsPartner { get; set; }

        /// <summary>
        /// Je klient druhem/družkou?
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("maritalStatus")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public LoanApplicationCounterPartyMaritalStatus MaritalStatus { get; set; }

    }

    /// <summary>
    /// Parametry domácnosti.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class LoanApplicationHousehold
    {
        /// <summary>
        /// počet vyživovaných dětí do 10 let (včetně).
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("childrenUnderAnd10")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public long ChildrenUnderAnd10 { get; set; }

        /// <summary>
        /// počet vyživovaných dětí nad 10 let .
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("childrenOver10")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public long ChildrenOver10 { get; set; }

        /// <summary>
        /// Household Expenses Summary
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("expensesSummary")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<ExpensesSummary> ExpensesSummary { get; set; } = new System.Collections.ObjectModel.Collection<ExpensesSummary>();

        /// <summary>
        /// Shrnutí pasiv úvěru mimo domácnosti
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("creditLiabilitiesSummaryOut")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<CreditLiabilitiesSummary> CreditLiabilitiesSummaryOut { get; set; } = new System.Collections.ObjectModel.Collection<CreditLiabilitiesSummary>();

        /// <summary>
        /// Shrnutí pasiv úvěru domácnosti
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("creditLiabilitiesSummary")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<CreditLiabilitiesSummaryHomeCompany> CreditLiabilitiesSummary { get; set; } = new System.Collections.ObjectModel.Collection<CreditLiabilitiesSummaryHomeCompany>();

        /// <summary>
        /// Shrnutí splátek mimo domácnosti
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("installmentsSummaryOut")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<InstallmentsSummaryOutHomeCompany> InstallmentsSummaryOut { get; set; } = new System.Collections.ObjectModel.Collection<InstallmentsSummaryOutHomeCompany>();

        /// <summary>
        /// Shrnutí splátek domácnosti
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("installmentsSummary")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<InstallmentsSummaryHomeCompany> InstallmentsSummary { get; set; } = new System.Collections.ObjectModel.Collection<InstallmentsSummaryHomeCompany>();

        /// <summary>
        /// Klienti
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("clients")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<LoanApplicationCounterParty> Clients { get; set; } = new System.Collections.ObjectModel.Collection<LoanApplicationCounterParty>();

    }

    /// <summary>
    /// Žádost o půjčku příjem.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class LoanApplicationIncome
    {
        /// <summary>
        /// Kategorie.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("category")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public LoanApplicationIncomeCategory Category { get; set; }

        /// <summary>
        /// Počet měsíců.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("month")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public int Month { get; set; }

        /// <summary>
        /// Množství.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amount")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public decimal Amount { get; set; }

    }

    /// <summary>
    /// Parametry potřebné pro výpočet Bonity
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class LoanApplicationProduct
    {
        /// <summary>
        /// kód produktového shluku (shluk jednoho produktu).
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("productClusterCode")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string ProductClusterCode { get; set; }

        /// <summary>
        /// Splatnost úvěru - počet splátek v měsíci.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("maturity")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public long? Maturity { get; set; }

        /// <summary>
        /// Žádaná roční úroková sazba.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("interestRate")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public decimal? InterestRate { get; set; }

        /// <summary>
        /// Požadovaná výše úvěru v Kč.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("amountRequired")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        public long AmountRequired { get; set; }

        /// <summary>
        /// Požadovaná splátka v Kč.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("annuity")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public long? Annuity { get; set; }

        /// <summary>
        /// Doba fixace v měsících.
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("fixationPeriod")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        public long? FixationPeriod { get; set; }

    }

    /// <summary>
    /// OrganizationUnit
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class OrganizationUnit
    {
        /// <summary>
        /// Pobočka + expozitura přihlášení uživatele/schvalovatele
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Id { get; set; }

        /// <summary>
        /// Název pobočky přihlášení uživatele/schvalovatele
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("name")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        /// <summary>
        /// Pracovní skupina
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("jobPost")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public JobPost JobPost { get; set; } = new JobPost();

    }

    /// <summary>
    /// Person
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class Person
    {
        /// <summary>
        /// osobní číslo přihlášeného uživatele/schvalovatele
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public ResourceIdentifier Id { get; set; } = new ResourceIdentifier();

        /// <summary>
        /// příjmení přihlášeného uživatele/příjmení schvalovatele
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("surname")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Surname { get; set; }

        /// <summary>
        /// Pracovní skupina
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("orgUnit")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required]
        public OrganizationUnit OrgUnit { get; set; } = new OrganizationUnit();

    }

    /// <summary>
    /// ResourceIdentifier
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal partial class ResourceIdentifier
    {
        /// <summary>
        /// The resource instance code, eg. 'KBCZ'
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("instance")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.StringLength(16)]
        public string Instance { get; set; }

        /// <summary>
        /// The resource domain code, eg. 'CFLM'
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("domain")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.StringLength(16)]
        public string Domain { get; set; }

        /// <summary>
        /// The resource code (the in-domain resource code, eg. 'LimitModel')
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("resource")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.StringLength(64)]
        public string Resource { get; set; }

        /// <summary>
        /// ID
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("id")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]   
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.StringLength(64)]
        public string Id { get; set; }

        /// <summary>
        /// The variant of the resource, eg. distinguishing the origin of the source
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("variant")]

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]   
        [System.ComponentModel.DataAnnotations.StringLength(16)]
        public string Variant { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal enum CreditLiabilitiesSummaryProductGroup
    {

        [System.Runtime.Serialization.EnumMember(Value = @"CC")]
        CC = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"AD")]
        AD = 1,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal enum CreditLiabilitiesSummaryHomeCompanyProductGroup
    {

        [System.Runtime.Serialization.EnumMember(Value = @"CC")]
        CC = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"AD")]
        AD = 1,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal enum CreditWorthinessCalculationArgumentsItChannel
    {

        [System.Runtime.Serialization.EnumMember(Value = @"NOBY")]
        NOBY = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"STARBUILD")]
        STARBUILD = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"DCS")]
        DCS = 2,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal enum ExpensesSummaryCategory
    {

        [System.Runtime.Serialization.EnumMember(Value = @"RENT")]
        RENT = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"ALIMONY")]
        ALIMONY = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"OTHER")]
        OTHER = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"INSURANCE")]
        INSURANCE = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"SAVING")]
        SAVING = 4,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal enum InstallmentsSummaryHomeCompanyProductGroup
    {

        [System.Runtime.Serialization.EnumMember(Value = @"CL")]
        CL = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"ML")]
        ML = 1,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal enum InstallmentsSummaryOutHomeCompanyProductGroup
    {

        [System.Runtime.Serialization.EnumMember(Value = @"CL")]
        CL = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"ML")]
        ML = 1,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal enum LoanApplicationCounterPartyMaritalStatus
    {

        [System.Runtime.Serialization.EnumMember(Value = @"S")]
        S = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"M")]
        M = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"D")]
        D = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"W")]
        W = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"R")]
        R = 4,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    internal enum LoanApplicationIncomeCategory
    {

        [System.Runtime.Serialization.EnumMember(Value = @"SALARY")]
        SALARY = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"ENTERPRISE")]
        ENTERPRISE = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"RENT")]
        RENT = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"OTHER")]
        OTHER = 3,

    }



    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
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