namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest;

internal class Obligation
{
    private readonly bool _obligationTypeExists;

    public required int Number { get; init; }

    public required DomainServices.HouseholdService.Contracts.Obligation ObligationData { get; init; }

    public required IEnumerable<int> ObligationTypeIds
    {
        init => _obligationTypeExists = value.Contains(ObligationData.ObligationTypeId!.Value);
    }

    public decimal? Correction =>
        _obligationTypeExists ? ObligationData.Correction?.LoanPrincipalAmountCorrection : ObligationData.Correction?.CreditCardLimitCorrection;

    public bool PartialCorrection
    {
        get
        {
            if (ObligationData.ObligationTypeId == 3)
                return false;

            if (_obligationTypeExists)
            {
                return (decimal?)ObligationData.AmountConsolidated < ObligationData.LoanPrincipalAmount;
            }

            return (decimal?)ObligationData.AmountConsolidated < ObligationData.CreditCardLimit;
        }
    }
}