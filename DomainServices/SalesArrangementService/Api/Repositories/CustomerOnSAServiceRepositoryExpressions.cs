using System.Linq.Expressions;

namespace DomainServices.SalesArrangementService.Api.Repositories;

internal static class CustomerOnSAServiceRepositoryExpressions
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
            CustomerRoleId = (int)t.CustomerRoleId
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
            IncomeSource = t.IncomeSource ?? ""
        };
    }
}