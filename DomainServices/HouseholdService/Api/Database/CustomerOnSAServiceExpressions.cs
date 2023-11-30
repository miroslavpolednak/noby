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
}