using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication;

public class LoanApplicationObligation
{
    private readonly Obligation _obligation;
    private readonly List<int> _obligationTypes;

    public LoanApplicationObligation(Obligation obligation, List<int> obligationTypes)
    {
        _obligation = obligation;
        _obligationTypes = obligationTypes;
    }

    public int ObligationTypeId => _obligation.ObligationTypeId!.Value;

    public decimal? Amount => _obligationTypes.Contains(_obligation.ObligationTypeId!.Value) ? _obligation.LoanPrincipalAmount : _obligation.CreditCardLimit;

    public decimal? AmountConsolidated => _obligationTypes.Contains(_obligation.ObligationTypeId!.Value) ? _obligation.Correction.LoanPrincipalAmountCorrection : _obligation.Correction.CreditCardLimitCorrection;

    public decimal? Installment => _obligation.InstallmentAmount;

    public decimal? InstallmentConsolidated => _obligation.Correction.InstallmentAmountCorrection;
}