namespace FOMS.Api.Endpoints.CustomerObligation;

internal static class Extensions
{
    public static Dto.ObligationFullDto ToApiResponse(this DomainServices.HouseholdService.Contracts.Obligation obligationInstance)
    {
        var model = new Dto.ObligationFullDto
        {
            CustomerOnSAId = obligationInstance.CustomerOnSAId,
            ObligationState = obligationInstance.ObligationState,
            CreditCardLimit = obligationInstance.CreditCardLimit,
            InstallmentAmount = obligationInstance.InstallmentAmount,
            ObligationId = obligationInstance.ObligationId,
            ObligationTypeId = obligationInstance.ObligationTypeId,
            LoanPrincipalAmount = obligationInstance.LoanPrincipalAmount,
            LoanPrincipalAmountConsolidated = obligationInstance.LoanPrincipalAmountConsolidated,
        };
        if (obligationInstance.Correction is not null)
            model.Correction = new Dto.ObligationCorrectionDto
            {
                CorrectionTypeId = obligationInstance.Correction.CorrectionTypeId,
                CreditCardLimitCorrection = obligationInstance.Correction.CreditCardLimitCorrection,
                InstallmentAmountCorrection = obligationInstance.Correction.InstallmentAmountCorrection,
                LoanPrincipalAmountCorrection = obligationInstance.Correction.LoanPrincipalAmountCorrection
            };
        if (obligationInstance.Creditor is not null)
            model.Creditor = new Dto.ObligationCreditorDto
            {
                CreditorId = obligationInstance.Creditor.CreditorId,
                IsExternal = obligationInstance.Creditor.IsExternal,
                Name = obligationInstance.Creditor.Name,
            };

        return model;
    }
}
