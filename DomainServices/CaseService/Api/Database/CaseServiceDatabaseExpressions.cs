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
            StateUpdatedInStarbuild = (Contracts.UpdatedInStarbuildStates)t.StateUpdatedInStarbuild,
            CaseOwner = new SharedTypes.GrpcTypes.UserInfo(t.OwnerUserId, t.OwnerUserName),
            Data = new()
            {
                ProductTypeId = t.ProductTypeId,
                TargetAmount = t.TargetAmount,
                IsEmployeeBonusRequested = t.IsEmployeeBonusRequested,
                ContractNumber = t.ContractNumber ?? ""
            },
            Customer = new()
            {
                Identity = !t.CustomerIdentityId.HasValue ? null : new SharedTypes.GrpcTypes.Identity(t.CustomerIdentityId, t.CustomerIdentityScheme),
                DateOfBirthNaturalPerson = t.DateOfBirthNaturalPerson,
                FirstNameNaturalPerson = t.FirstNameNaturalPerson ?? "",
                Name = t.Name ?? "",
                Cin = t.Cin ?? "",
                CustomerPriceSensitivity = t.CustomerPriceSensitivity,
                CustomerChurnRisk = t.CustomerChurnRisk
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
            Created = new SharedTypes.GrpcTypes.ModificationStamp(t.CreatedUserId, t.CreatedUserName, t.CreatedTime)
        };
    }
}
