using System.Linq.Expressions;

namespace DomainServices.HouseholdService.Api.Database;

internal static class CustomerOnSAServiceExpressions
{
    public static Expression<Func<Entities.CustomerOnSA, Contracts.CustomerOnSA>> CustomerDetail()
    {
        return t => new Contracts.CustomerOnSA
        {
            CustomerOnSAId = t.CustomerOnSAId,
            Name = t.Name,
            FirstNameNaturalPerson = t.FirstNameNaturalPerson,
            DateOfBirthNaturalPerson = t.DateOfBirthNaturalPerson,
            SalesArrangementId = t.SalesArrangementId,
            CustomerRoleId = (int)t.CustomerRoleId,
            LockedIncomeDateTime = t.LockedIncomeDateTime,
            MaritalStatusId = t.MaritalStatusId
        };
    }

    public static Expression<Func<Entities.CustomerOnSAIncome, Contracts.IncomeInList>> Income()
    {
        return t => new Contracts.IncomeInList
        {
            IncomeId = t.CustomerOnSAIncomeId,
            IncomeTypeId = (int)t.IncomeTypeId,
            CurrencyCode = t.CurrencyCode ?? "",
            Sum = t.Sum,
            IncomeSource = t.IncomeSource ?? "",
            HasProofOfIncome = t.HasProofOfIncome
        };
    }

    public static Expression<Func<Entities.CustomerOnSAObligation, Contracts.Obligation>> Obligation()
    {
        return entity => new Contracts.Obligation
        {
            CustomerOnSAId = entity.CustomerOnSAId,
            ObligationState = entity.ObligationState,
            InstallmentAmount = entity.InstallmentAmount,
            CreditCardLimit = entity.CreditCardLimit,
            LoanPrincipalAmount = entity.LoanPrincipalAmount,
            AmountConsolidated = entity.AmountConsolidated,
            ObligationTypeId = entity.ObligationTypeId,
            ObligationId = entity.CustomerOnSAObligationId,
            Creditor = new Contracts.ObligationCreditor
            {
                CreditorId = entity.CreditorId ?? "",
                IsExternal = entity.CreditorIsExternal,
                Name = entity.CreditorName ?? ""
            },
            Correction = new Contracts.ObligationCorrection
            {
                CorrectionTypeId = entity.CorrectionTypeId,
                CreditCardLimitCorrection = entity.CreditCardLimitCorrection,
                InstallmentAmountCorrection = entity.InstallmentAmountCorrection,
                LoanPrincipalAmountCorrection = entity.LoanPrincipalAmountCorrection
            }
        };
    }
}