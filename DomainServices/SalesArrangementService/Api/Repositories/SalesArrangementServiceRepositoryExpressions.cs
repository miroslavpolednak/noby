using System.Linq.Expressions;

namespace DomainServices.SalesArrangementService.Api.Repositories;

internal static class SalesArrangementServiceRepositoryExpressions
{
    public static Expression<Func<Entities.SalesArrangement, Contracts.SalesArrangement>> SalesArrangementDetail()
    {
        return t => new Contracts.SalesArrangement
        {
            SalesArrangementId = t.SalesArrangementId,
            CaseId = t.CaseId,
            State = t.State,
            OfferId = t.OfferId,
            SalesArrangementTypeId = t.SalesArrangementTypeId,
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(t.CreatedUserId, t.CreatedUserName, t.CreatedTime)
        };
    }
}
