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
            FirstNameNaturalPerson = t.Name,
            DateOfBirthNaturalPerson = t.DateOfBirthNaturalPerson,
            SalesArrangementId = t.SalesArrangementId,
            CustomerRoleId = (int)t.CustomerRoleId
        };
    }
}