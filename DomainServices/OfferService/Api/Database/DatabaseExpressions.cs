using DomainServices.OfferService.Api.Database.Entities;
using DomainServices.OfferService.Contracts;
using System.Linq.Expressions;

namespace DomainServices.OfferService.Api.Database;

internal static class DatabaseExpressions
{
    public static Expression<Func<Offer, Contracts.CommonOfferData>> CreateCommonOfferData()
    {
        return t => new CommonOfferData
        {
            CaseId = t.CaseId,
            SalesArrangementId = t.SalesArrangementId,
            OfferId = t.OfferId,
            ResourceProcessId = t.ResourceProcessId.ToString(),
            DocumentId = t.DocumentId,
            OfferType = (OfferTypes)t.OfferType,
            Flags = t.Flags,
            ValidTo = t.ValidTo,
            Origin = (OfferOrigins)t.Origin,
            IsCreditWorthinessSimpleRequested = t.IsCreditWorthinessSimpleRequested,
            Created =  new SharedTypes.GrpcTypes.ModificationStamp
            {
                DateTime = t.CreatedTime,
                UserId = t.CreatedUserId == null ? null : t.CreatedUserId,
                UserName = t.CreatedUserName == null ? "" : t.CreatedUserName
            }
        };
    }
}
