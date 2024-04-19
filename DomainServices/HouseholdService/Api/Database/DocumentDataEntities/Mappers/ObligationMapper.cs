using CIS.Core.Attributes;
using __Contracts = DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;

#pragma warning disable CA1822 // Mark members as static
[TransientService, SelfService]
internal sealed class ObligationMapper
{
    public Obligation MapToData(__Contracts.IObligation? source)
    {
        if (source == null) return new Obligation();

        return new Obligation
        {
            ObligationState = source.ObligationState,
            AmountConsolidated = source.AmountConsolidated,
            CreditCardLimit = source.CreditCardLimit,
            InstallmentAmount = source.InstallmentAmount,
            LoanPrincipalAmount = source.LoanPrincipalAmount,
            ObligationTypeId = source.ObligationTypeId,
            Correction = source.Correction is null ? null : new()
            {
                CorrectionTypeId = source.Correction.CorrectionTypeId,
                CreditCardLimitCorrection = source.Correction.CreditCardLimitCorrection,
                InstallmentAmountCorrection = source.Correction.InstallmentAmountCorrection,
                LoanPrincipalAmountCorrection = source.Correction.LoanPrincipalAmountCorrection
            },
            Creditor = source.Creditor is null ? null : new()
            {
                CreditorId = source.Creditor.CreditorId,
                IsExternal = source.Creditor.IsExternal,
                Name = source.Creditor.Name
            }
        };
    }

    public __Contracts.Obligation MapFromDataToList(DocumentDataItem<Obligation, int> item)
    {
        var model = MapFromDataToSingle(item.Data) ?? new __Contracts.Obligation();
        model.ObligationId = item.DocumentDataStorageId;
        model.CustomerOnSAId = item.EntityId;
        return model;
    }

    public __Contracts.Obligation? MapFromDataToSingle(Obligation? data)
    {
        if (data is null) return new __Contracts.Obligation();

        return new __Contracts.Obligation
        {
            ObligationState = data.ObligationState,
            AmountConsolidated = data.AmountConsolidated,
            CreditCardLimit = data.CreditCardLimit,
            InstallmentAmount = data.InstallmentAmount,
            LoanPrincipalAmount = data.LoanPrincipalAmount,
            ObligationTypeId = data.ObligationTypeId,
            Correction = data.Correction is null ? null : new()
            {
                CorrectionTypeId = data.Correction.CorrectionTypeId,
                CreditCardLimitCorrection = data.Correction.CreditCardLimitCorrection,
                InstallmentAmountCorrection = data.Correction.InstallmentAmountCorrection,
                LoanPrincipalAmountCorrection = data.Correction.LoanPrincipalAmountCorrection
            },
            Creditor = data.Creditor is null ? null : new()
            {
                CreditorId = data.Creditor.CreditorId,
                IsExternal = data.Creditor.IsExternal,
                Name = data.Creditor.Name
            }
        };
    }
}
