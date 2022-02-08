using System.Linq.Expressions;

namespace DomainServices.CaseService.Api.Repositories;

internal static class CaseServiceRepositoryExpressions
{
    public static Expression<Func<Entities.CaseInstance, Contracts.Case>> CaseDetail()
    {
        return t => new Contracts.Case
        {
            CaseId = t.CaseId,
            State = t.State,
            StateUpdatedOn = t.StateUpdateTime,
            ActionRequired = t.IsActionRequired,
            CaseOwner = new CIS.Infrastructure.gRPC.CisTypes.UserInfo(t.OwnerUserId, t.OwnerUserName),
            Data = new Contracts.CaseData
            {
                ProductTypeId = t.ProductTypeId,
                TargetAmount = t.TargetAmount,
                ContractNumber = t.ContractNumber ?? ""
            },
            Customer = new Contracts.CustomerData
            {
                Identity = !t.CustomerIdentityId.HasValue ? null : new CIS.Infrastructure.gRPC.CisTypes.Identity(t.CustomerIdentityId, t.CustomerIdentityScheme),
                DateOfBirthNaturalPerson = t.DateOfBirthNaturalPerson,
                FirstNameNaturalPerson = t.FirstNameNaturalPerson ?? "",
                Name = t.Name ?? "",
                Cin = t.Cin ?? ""
            },
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(t.CreatedUserId, t.CreatedUserName, t.CreatedTime)
        };
    }
}
