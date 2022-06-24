namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail.Dto;

public class MortgageDetailDto
{
    public int LoanKindId { get; set; }

    /// <summary>
    /// ČÍslo smlouvy. Odpovídá smlouvě uzavřené pro zřízení stavebního spoření, nebo hypotéky. Úvěry ze SS mají jiné číslo smlouvy (jiný suffix), ale nezobrazuje se na case.
    /// </summary>
    public string? ContractNumber { get; set; }

    public int FixedRatePeriod { get; set; }

    public DateTime? ExpectedDateOfDrawing { get; set; }

    public DateTime? LoanDueDate { get; set; }

    public int? PaymentDay { get; set; }

    /// <summary>
    /// Akce / produkt
    /// </summary>
    public string ProductName { get; set; } = null!;
    
    /// <summary>
    /// Výše úvěru
    /// </summary>
    public decimal? LoanAmount { get; set; }
    
    /// <summary>
    /// Skládačková úroková sazba 
    /// </summary>
    public decimal LoanInterestRate { get; set; }
    
    /// <summary>
    /// Datum uzavreni smlouvy
    /// </summary>
    public DateTime? ContractStartDate { get; set; }
    
    /// <summary>
    /// Datum fixace
    /// </summary>
    public DateTime? FixationDate { get; set; }

    /// <summary>
    /// Zustatek na ucte
    /// </summary>
    public decimal? AccountBalance { get; set; }
    
    /// <summary>
    /// Zbyva cerpat
    /// </summary>
    public decimal? AmountToWithdraw { get; set; }
    
    /// <summary>
    /// Mesicni splatka
    /// </summary>
    public decimal? LoanPaymentAmount { get; set; }

    /// <summary>
    /// Datum cerpani
    /// </summary>
    public DateTime? DateOfDrawing { get; set; }
    
    /// <summary>
    /// Platne VUP od
    /// </summary>
    public DateTime? LoanTermsValidFrom { get; set; }
    
    /// <summary>
    /// Rocni vypis z uctu
    /// </summary>
    public bool YearlyAccountStatement { get; set; }

    public DateTime? ContractSignedDate { get; set; }
    public DateTime? FixedRateValidTo { get; set; }
    public decimal? Principal { get; set; }
    public decimal? AvailableForDrawing { get; set; }
    public DateTime? DrawingDateTo { get; set; }
    public decimal? CurrentAmount { get; set; }
    public string? PaymentAccount { get; set; }
    public decimal? CurrentOverdueAmount { get; set; }
    public decimal? AllOverdueFees { get; set; }
    public int? OverdueDaysNumber { get; set; }
    public decimal? InterestInArrears { get; set; }
    public decimal? LoanInterestRateRefix { get; set; }
    public DateTime? LoanInterestRateValidFromRefix { get; set; }
    public int? FixedRatePeriodRefix { get; set; }

    public List<MortgageDetailLoanPurpose>? LoanPurposes { get; set; }
}