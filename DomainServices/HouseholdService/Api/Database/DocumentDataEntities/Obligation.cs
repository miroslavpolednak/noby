namespace DomainServices.HouseholdService.Api.Database.DocumentDataEntities;

internal sealed class Obligation
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public int? ObligationTypeId { get; set; }
    public decimal? InstallmentAmount { get; set; }
    public decimal? LoanPrincipalAmount { get; set; }
    public decimal? CreditCardLimit { get; set; }
    public decimal? AmountConsolidated { get; set; }
    public int? ObligationState { get; set; }
    public ObligationCreditor? Creditor { get; set; }
    public ObligationCorrection? Correction { get; set; }

    internal sealed class ObligationCreditor
    {
        public string? CreditorId { get; set; }
        public string? Name { get; set; }
        public bool? IsExternal { get; set; }
    }

    internal sealed class ObligationCorrection
    {
        public int? CorrectionTypeId { get; set; }
        public decimal? InstallmentAmountCorrection { get; set; }
        public decimal? LoanPrincipalAmountCorrection { get; set; }
        public decimal? CreditCardLimitCorrection { get; set; }
    }
}
