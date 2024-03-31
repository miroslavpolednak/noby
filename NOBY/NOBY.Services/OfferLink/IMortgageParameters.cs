namespace NOBY.Services.OfferLink;

public interface IMortgageParameters
{
    long CaseId { get; init; }

    long TaskProcessId { get; init; }

    DateTime InterestRateValidFrom { get; }

    decimal? LoanInterestRateDiscount { get; }

    decimal LoanInterestRate { get; }

    decimal? LoanInterestRateProvided { get; }

    decimal? LoanPaymentAmount { get; }

    decimal? LoanPaymentAmountFinal { get; }

    decimal FeeSum { get; }

    decimal FeeFinalSum { get; }
}