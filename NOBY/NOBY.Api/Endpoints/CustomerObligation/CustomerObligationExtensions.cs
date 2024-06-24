using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerObligation;

internal static class CustomerObligationExtensions
{
    public static CustomerObligationObligationFull ToApiResponse(this _HO.Obligation obligationInstance)
    {
        return new()
        {
            CustomerOnSAId = obligationInstance.CustomerOnSAId,
            ObligationState = obligationInstance.ObligationState,
            CreditCardLimit = obligationInstance.CreditCardLimit,
            InstallmentAmount = obligationInstance.InstallmentAmount,
            ObligationId = obligationInstance.ObligationId,
            ObligationTypeId = obligationInstance.ObligationTypeId,
            LoanPrincipalAmount = obligationInstance.LoanPrincipalAmount,
            AmountConsolidated = obligationInstance.AmountConsolidated,
            Correction = obligationInstance.Correction is null ? null : new()
            {
                CorrectionTypeId = obligationInstance.Correction.CorrectionTypeId,
                CreditCardLimitCorrection = obligationInstance.Correction.CreditCardLimitCorrection,
                InstallmentAmountCorrection = obligationInstance.Correction.InstallmentAmountCorrection,
                LoanPrincipalAmountCorrection = obligationInstance.Correction.LoanPrincipalAmountCorrection
            },
            Creditor = obligationInstance.Creditor is null ? null : new()
            {
                CreditorId = obligationInstance.Creditor.CreditorId,
                IsExternal = obligationInstance.Creditor.IsExternal,
                Name = obligationInstance.Creditor.Name,
            }
        };
    }

    public static _HO.ObligationCorrection? ToDomainServiceRequest(this CustomerObligationSharedCorrection? correction)
    {
        return correction is null ? null : new()
        {
            CorrectionTypeId = correction.CorrectionTypeId,
            CreditCardLimitCorrection = correction.CreditCardLimitCorrection,
            InstallmentAmountCorrection = correction.InstallmentAmountCorrection,
            LoanPrincipalAmountCorrection = correction.LoanPrincipalAmountCorrection
        };
    }

    public static _HO.ObligationCreditor? ToDomainServiceRequest(this CustomerObligationSharedCreditor? creditor)
    {
        return creditor is null ? null : new()
        {
            CreditorId = creditor.CreditorId ?? "",
            IsExternal = creditor.IsExternal,
            Name = creditor.Name ?? ""
        };
    }
}
