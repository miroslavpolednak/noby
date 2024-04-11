using DomainServices.CaseService.Contracts;

namespace NOBY.Services.MortgageRefinancing;

public class MortgageRefinancingIndividualPrice
{
    public MortgageRefinancingIndividualPrice(decimal? loanInterestRateDiscount, decimal? feeFinalSum)
    {
        LoanInterestRateDiscount = loanInterestRateDiscount;
        FeeFinalSum = feeFinalSum;
    }

    public MortgageRefinancingIndividualPrice(AmendmentPriceException priceException)
    {
        LoanInterestRateDiscount = priceException.LoanInterestRate.LoanInterestRateDiscount;
        FeeFinalSum = priceException.Fees.FirstOrDefault()?.FinalSum;
    }

    public bool HasIndividualPrice => LoanInterestRateDiscount != 0 || FeeFinalSum != 0;

    public decimal? LoanInterestRateDiscount { get; init; }

    public decimal? FeeFinalSum { get; init; }

    public override bool Equals(object? obj) => 
        Equals(obj as MortgageRefinancingIndividualPrice);

    public virtual bool Equals(MortgageRefinancingIndividualPrice? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return LoanInterestRateDiscount.GetValueOrDefault() == other.LoanInterestRateDiscount.GetValueOrDefault() &&
               FeeFinalSum.GetValueOrDefault() == other.FeeFinalSum.GetValueOrDefault();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LoanInterestRateDiscount, FeeFinalSum);
    }
}