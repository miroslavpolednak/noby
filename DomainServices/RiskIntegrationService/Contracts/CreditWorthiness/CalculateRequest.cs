namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

/// <summary>
/// Parametry potřebné pro výpočet Bonity
/// </summary>
[DataContract]
public class CalculateRequest
    : IRequest<CalculateResponse>, CIS.Core.Validation.IValidatableRequest
{
    /// <summary>
    /// ResourceProcessId
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public string? ResourceProcessIdMp { get; set; }

    /// <summary>
    /// Identifikátor volající aplikace.
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public string? ItChannel { get; set; }

    /// <summary>
    /// Identifikátor žádosti z pohledu Risku (nepovinné).
    /// </summary>
    [DataMember(Order = 3)]
    public string? RiskBusinessCaseIdMp { get; set; }

    /// <summary>
    /// Uživatel/schvalovatel, který službu volá (přihlášený do NOBY/StarBuildu)
    /// </summary>
    [Required]
    [DataMember(Order = 4)]
    public HumanUser? HumanUser { get; set; }

    /// <summary>
    /// Loan application product.
    /// </summary>
    [Required]
    [DataMember(Order = 5)]
    public LoanApplicationProduct? LoanApplicationProduct { get; set; }

    /// <summary>
    /// Domácnosti.
    /// </summary>
    /// <value>Domácnosti.</value>
    [Required]
    [DataMember(Order = 6)]
    public List<LoanApplicationHousehold>? Households { get; set; }
}

