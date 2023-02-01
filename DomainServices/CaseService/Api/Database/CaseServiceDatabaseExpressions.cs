using System.Linq.Expressions;
using DomainServices.CaseService.Api.Database.Entities;

namespace DomainServices.CaseService.Api.Database;

internal static class CaseServiceDatabaseExpressions
{
    public static Expression<Func<Case, Contracts.Case>> CaseDetail()
    {
        return t => new Contracts.Case
        {
            CaseId = t.CaseId,
            State = t.State,
            StateUpdatedOn = t.StateUpdateTime,
            CaseOwner = new CIS.Infrastructure.gRPC.CisTypes.UserInfo(t.OwnerUserId, t.OwnerUserName),
            Data = new()
            {
                ProductTypeId = t.ProductTypeId,
                TargetAmount = t.TargetAmount,
                ContractNumber = t.ContractNumber ?? ""
            },
            Customer = new()
            {
                Identity = !t.CustomerIdentityId.HasValue ? null : new CIS.Infrastructure.gRPC.CisTypes.Identity(t.CustomerIdentityId, t.CustomerIdentityScheme),
                DateOfBirthNaturalPerson = t.DateOfBirthNaturalPerson,
                FirstNameNaturalPerson = t.FirstNameNaturalPerson ?? "",
                Name = t.Name ?? "",
                Cin = t.Cin ?? ""
            },
            OfferContacts = new()
            {
                EmailForOffer = t.EmailForOffer ?? "",
                PhoneNumberForOffer = new()
                {
                    PhoneNumber = t.PhoneNumberForOffer ?? "",
                    PhoneIDC = t.PhoneIDCForOffer ?? ""
                }
            },
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(t.CreatedUserId, t.CreatedUserName, t.CreatedTime)
        };
    }
}
