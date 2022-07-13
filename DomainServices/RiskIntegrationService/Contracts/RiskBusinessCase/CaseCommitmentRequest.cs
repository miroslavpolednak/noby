namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[DataContract]
public class CaseCommitmentRequest
    : IRequest<CaseCommitmentResponse>, CIS.Core.Validation.IValidatableRequest
{
    [DataMember(Order = 13)]
    public string RiskBusinessCaseId { get; set; }

    /// <summary>
    /// ID dané úvěrové žádosti.
    /// </summary>
    [DataMember(Order = 1)]
    public SystemId loanApplicationId { get; set; }

    /// <summary>
    /// IT Channel.
    /// </summary>
    [DataMember(Order = 2)]
    public string ItChannel { get; set; }

    /// <summary>
    /// loanApplicationProduct.
    /// </summary>
    [DataMember(Order = 3)]
    public LoanApplicationProduct LoanApplicationProduct { get; set; }

    /// <summary>
    /// riskBusinessCaseFinalResult.
    /// </summary>
    [DataMember(Order = 4)]
    public string RiskBusinessCaseFinalResult { get; set; }

    /// <summary>
    /// loanSoldProduct.
    /// </summary>
    [DataMember(Order = 5)]
    public LoanSoldProduct LoanSoldProduct { get; set; }

    /// <summary>
    /// approvalLevel.
    /// </summary>
    [DataMember(Order = 6)]
    public string ApprovalLevel { get; set; }

    /// <summary>
    /// Datum schválení.  Format: yyyy-MM-dd 
    /// </summary>
    [DataMember(Order = 7)]
    public DateTime? ApprovalDate { get; set; }

    /// <summary>
    /// loanAgreement.
    /// </summary>
    [DataMember(Order = 8)]
    public LoanAgreement LoanAgreement { get; set; }

    /// <summary>
    /// approver.
    /// </summary>
    [DataMember(Order = 9)]
    public Identity Approver { get; set; }

    /// <summary>
    /// collateralAgreements.
    /// </summary>
    [DataMember(Order = 10)]
    public List<CollateralAgreement> CollateralAgreements { get; set; }

    /// <summary>
    /// Uživatel/schvalovatel, který službu volá (přihlášený do NOBY/StarBuildu)
    /// </summary>
    [DataMember(Order = 11)]
    public Identity HumanUser { get; set; }
}
