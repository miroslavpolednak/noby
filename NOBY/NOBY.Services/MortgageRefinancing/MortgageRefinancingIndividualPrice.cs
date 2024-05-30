using DomainServices.CaseService.Contracts;

namespace NOBY.Services.MortgageRefinancing;

public class MortgageRefinancingIndividualPrice
{
    public MortgageRefinancingIndividualPrice(decimal? loanInterestRateDiscount, decimal? feeDiscount)
    {
        LoanInterestRateDiscount = loanInterestRateDiscount;
        FeeDiscount = feeDiscount;
    }

    public MortgageRefinancingIndividualPrice(AmendmentPriceException priceException)
    {
        LoanInterestRateDiscount = priceException.LoanInterestRate.LoanInterestRateDiscount;
        FeeDiscount = GetFeeDiscount(priceException);
    }

    public bool HasIndividualPrice => LoanInterestRateDiscount.GetValueOrDefault() != 0 || FeeDiscount.GetValueOrDefault() != 0;

    public decimal? LoanInterestRateDiscount { get; init; }

    public decimal? FeeDiscount { get; init; }

    public override bool Equals(object? obj) => 
        Equals(obj as MortgageRefinancingIndividualPrice);

    public virtual bool Equals(MortgageRefinancingIndividualPrice? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return LoanInterestRateDiscount.GetValueOrDefault() == other.LoanInterestRateDiscount.GetValueOrDefault() &&
               FeeDiscount.GetValueOrDefault() == other.FeeDiscount.GetValueOrDefault();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LoanInterestRateDiscount, FeeDiscount);
    }

    private static decimal? GetFeeDiscount(AmendmentPriceException priceException)
    {
        var fee = priceException.Fees.FirstOrDefault();

        if (fee is null)
            return default;

        return fee.FinalSum - fee.TariffSum;
    }
}