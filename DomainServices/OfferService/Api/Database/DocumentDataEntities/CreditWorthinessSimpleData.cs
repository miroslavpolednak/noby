namespace DomainServices.OfferService.Api.Database.DocumentDataEntities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

internal sealed class CreditWorthinessSimpleData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public CreditWorhinessInputs Inputs { get; set; }
    public CreditWorhinessOutputs Outputs { get; set; }

    public sealed class CreditWorhinessInputs
    {
        public decimal? TotalMonthlyIncome { get; set; }
        public ExpensesSummaryData ExpensesSummary { get; set; }
        public ObligationsSummaryData ObligationsSummary { get; set; }
        public int? ChildrenCount { get; set; }
    }

    public sealed class CreditWorhinessOutputs
    {
        public int? InstallmentLimit { get; set; }
        public int? MaxAmount { get; set; }
        public int? RemainsLivingAnnuity { get; set; }
        public int? RemainsLivingInst { get; set; }
        public WorthinessResults WorthinessResult { get; set; }
    }

    public enum WorthinessResults
    {
        Unknown = 0,
        Success = 1,
        Failed = 2
    }

    public sealed class ExpensesSummaryData
    {
        public decimal? Rent { get; set; }
        public decimal? Other { get; set; }
    }

    public sealed class ObligationsSummaryData
    {
        public decimal? LoansInstallmentsAmount { get; set; }
        public decimal? CreditCardsAmount { get; set; }
        public decimal? AuthorizedOverdraftsTotalAmount { get; set; }
    }
}
