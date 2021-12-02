namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class BuildingSavingsData
{
    /// <summary>
    /// Doba spoření zaokrouhlená na celá čísla (double → Int)
    /// </summary>
    public int SavingPeriod { get; set; }

    /// <summary>
    /// Datum předpokládaného ukončení smlouvy o SS
    /// </summary>
    public DateTime ContractTerminationDate { get; set; }

    /// <summary>
    /// Úroková sazba
    /// </summary>
    public decimal InterestRate { get; set; }

    /// <summary>
    /// Kolik bude naspořeno na konci spořící periody
    /// </summary>
    public decimal TotalSaved { get; set; }

    public decimal TotalDeposits { get; set; }
    public decimal TotalInterests { get; set; }
    public decimal TotalFees { get; set; }
    public decimal InterestsAdvantage { get; set; }

    /// <summary>
    /// Součet státní podpora celkem a evidovaná státní podpora
    /// </summary>
    public decimal TotalGovernmentIncentives { get; set; }

    public DateTime? ExpectedGrantedLoanDate { get; set; }
    public decimal DepositBalance { get; set; }
    public decimal BenefitInterests { get; set; }
    public decimal TaxFromBenefitInterests { get; set; }
    
    public static implicit operator BuildingSavingsData(DomainServices.OfferService.Contracts.BuildingSavingsData data)
        => new BuildingSavingsData
        {
            SavingPeriod = data.SavingPeriod,
            ContractTerminationDate = data.ContractTerminationDate,
            InterestRate = data.InterestRate,
            TotalSaved = data.TotalSaved,
            TotalDeposits = data.TotalDeposits,
            TotalInterests = data.TotalInterests,
            TotalFees = data.TotalFees,
            InterestsAdvantage = data.InterestsAdvantage,
            TotalGovernmentIncentives = data.TotalGovernmentIncentives,
            ExpectedGrantedLoanDate = data.ExpectedGrantedLoanDate,
            DepositBalance = data.DepositBalance,
            BenefitInterests = data.BenefitInterests,
            TaxFromBenefitInterests = data.TaxFromBenefitInterests
        };
}
