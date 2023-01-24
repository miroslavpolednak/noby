using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;

internal class LoanApplicationObligation
{
    private readonly ILookup<int?, Obligation> _obligationsByType;

    public LoanApplicationObligation(CustomerOnSA customerOnSa)
    {
        _obligationsByType = customerOnSa.Obligations.ToLookup(o => o.ObligationTypeId);
    }

    public decimal ObligationMLInstallment => ObligationSum(1, x => x.InstallmentAmount);

    public decimal ObligationMLLoanPrincipal => ObligationSum(1, x => x.LoanPrincipalAmount);

    public decimal ObligationMLAmountConsolidated => ObligationSum(1, x => x.AmountConsolidated);

    public decimal ObligationMLSumWithCorrection => ObligationSumWithCorrection(1);

    public decimal ObligationML2Installment => ObligationSum(5, x => x.InstallmentAmount);

    public decimal ObligationML2LoanPrincipal => ObligationSum(5, x => x.LoanPrincipalAmount);

    public decimal ObligationML2AmountConsolidated => ObligationSum(5, x => x.AmountConsolidated);

    public decimal ObligationML2SumWithCorrection => ObligationSumWithCorrection(5);

    public decimal ObligationCLInstallment => ObligationSum(2, x => x.InstallmentAmount);

    public decimal ObligationCLLoanPrincipal => ObligationSum(2, x => x.LoanPrincipalAmount);

    public decimal ObligationCLAmountConsolidated => ObligationSum(2, x => x.AmountConsolidated);

    public decimal ObligationCLSumWithCorrection => ObligationSumWithCorrection(2);

    public decimal CreditCardLimitCC => ObligationSum(3, x => x.CreditCardLimit);

    public decimal CreditCardCorrectionCC => CreditCardCorrection(3);

    public decimal ObligationCCAmountConsolidated => ObligationSum(3, x => x.AmountConsolidated);

    public decimal CreditCardCorrectionConsolidatedCC => CreditCardCorrectionConsolidated(3);

    public decimal CreditCardLimitAD => ObligationSum(4, x => x.CreditCardLimit);

    public decimal CreditCardCorrectionAD => CreditCardCorrection(4);

    public decimal ObligationADAmountConsolidated => ObligationSum(4, x => x.AmountConsolidated);

    public decimal CreditCardCorrectionConsolidatedAD => CreditCardCorrectionConsolidated(4);

    private decimal ObligationSum(int obligationTypeId, Func<Obligation, NullableGrpcDecimal> propertySelector)
    {
        return _obligationsByType[obligationTypeId].Select(propertySelector).Select(d => ((decimal?)d) ?? 0M).Sum();
    }

    private decimal ObligationSumWithCorrection(int obligationTypeId)
    {
        var correction2 = _obligationsByType[obligationTypeId].Where(o => o.Correction.CorrectionTypeId == 2)
                                                              .Select(o => ((decimal?)o.LoanPrincipalAmount) ?? 0M)
                                                              .Sum();

        var obligationWithCorrection3 = _obligationsByType[obligationTypeId].Where(o => o.Correction.CorrectionTypeId == 3).ToList();

        var correction3 = obligationWithCorrection3.Select(o => ((decimal?)o.LoanPrincipalAmount) ?? 0M).Sum() -
                          obligationWithCorrection3.Select(o => ((decimal?)o.AmountConsolidated) ?? 0M).Sum();

        return correction2 + correction3;
    }

    private decimal CreditCardCorrection(int obligationTypeId)
    {
        var correction1 = _obligationsByType[obligationTypeId].Where(o => o.Correction.CorrectionTypeId == 1).Select(o => (decimal?)o.CreditCardLimit ?? 0M).Sum();

        var obligationsWithCorrection4 = _obligationsByType[obligationTypeId].Where(o => o.Correction.CorrectionTypeId == 4).ToList();

        var correction4 = obligationsWithCorrection4.Select(o => (decimal?)o.CreditCardLimit ?? 0M).Sum() -
                          obligationsWithCorrection4.Select(o => (decimal?)o.Correction.CreditCardLimitCorrection ?? 0M).Sum();

        return correction1 + correction4;
    }

    private decimal CreditCardCorrectionConsolidated(int obligationTypeId)
    {
        var correction2 = _obligationsByType[obligationTypeId].Where(o => o.Correction.CorrectionTypeId == 2).Select(o => (decimal?)o.CreditCardLimit ?? 0M).Sum();

        var obligationsWithCorrection3 = _obligationsByType[obligationTypeId].Where(o => o.Correction.CorrectionTypeId == 3).ToList();

        var correction3 = obligationsWithCorrection3.Select(o => (decimal?)o.CreditCardLimit ?? 0M).Sum() -
                          obligationsWithCorrection3.Select(o => (decimal?)o.AmountConsolidated ?? 0M).Sum();

        var correction4 = _obligationsByType[obligationTypeId].Where(o => o.Correction.CorrectionTypeId == 4).Select(o => (decimal?)o.Correction.CreditCardLimitCorrection ?? 0M).Sum();

        return correction2 + correction3 + correction4;
    }
}