namespace FOMS.Api.Endpoints.Offer.Dto;

public class MortgageOutputs
{
    /// <summary>
    /// Výše úvěru
    /// </summary>
    public decimal? LoanAmount { get; set; }

    public DateTime? LoanDueDate { get; set; }
    public int? LoanDuration { get; set; }
    public decimal? LoanPaymentAmount { get; set; }
    public int? EmployeeBonusLoanCode { get; set; }
    public decimal LoanToValue { get; set; }
    public decimal Aprc { get; set; }
    public decimal LoanTotalAmount { get; set; }
    public decimal LoanInterestRateProvided { get; set; }
    public DateTime? ContractSignedDate { get; set; }
    public DateTime DrawingDateTo { get; set; }
    public DateTime? AnnuityPaymentsDateFrom { get; set; }
    public int? AnnuityPaymentsCount { get; set; }
    public decimal LoanInterestRate { get; set; }
    public decimal LoanInterestRateAnnounced { get; set; }
    public int LoanInterestRateAnnouncedType { get; set; }
    public decimal EmployeeBonusDeviation { get; set; }
    public decimal MarketingActionsDeviation { get; set; }

    /// <summary>
    /// Den splátky úvěru
    /// </summary>
    public int? PaymentDay { get; set; }

    public List<LoanPurposeItem>? LoanPurposes { get; set; }

    public List<MarketingActionItem>? MarketingActions { get; set; }

    public List<PaymentScheduleSimpleItem>? PaymentScheduleSimple { get; set; }
}