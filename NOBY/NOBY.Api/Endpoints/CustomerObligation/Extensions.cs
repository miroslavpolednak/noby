﻿namespace NOBY.Api.Endpoints.CustomerObligation;

internal static class Extensions
{
    public static SharedDto.ObligationFullDto ToApiResponse(this DomainServices.HouseholdService.Contracts.Obligation obligationInstance)
    {
        var model = new SharedDto.ObligationFullDto
        {
            CustomerOnSAId = obligationInstance.CustomerOnSAId,
            ObligationState = obligationInstance.ObligationState,
            CreditCardLimit = obligationInstance.CreditCardLimit,
            InstallmentAmount = obligationInstance.InstallmentAmount,
            ObligationId = obligationInstance.ObligationId,
            ObligationTypeId = obligationInstance.ObligationTypeId,
            LoanPrincipalAmount = obligationInstance.LoanPrincipalAmount,
            AmountConsolidated = obligationInstance.AmountConsolidated
        };
        if (obligationInstance.Correction is not null)
            model.Correction = new SharedDto.ObligationCorrectionDto
            {
                CorrectionTypeId = obligationInstance.Correction.CorrectionTypeId,
                CreditCardLimitCorrection = obligationInstance.Correction.CreditCardLimitCorrection,
                InstallmentAmountCorrection = obligationInstance.Correction.InstallmentAmountCorrection,
                LoanPrincipalAmountCorrection = obligationInstance.Correction.LoanPrincipalAmountCorrection
            };
        if (obligationInstance.Creditor is not null)
            model.Creditor = new SharedDto.ObligationCreditorDto
            {
                CreditorId = obligationInstance.Creditor.CreditorId,
                IsExternal = obligationInstance.Creditor.IsExternal,
                Name = obligationInstance.Creditor.Name,
            };

        return model;
    }
}
