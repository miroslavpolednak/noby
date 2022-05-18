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
            ContractNumber = t.ContractNumber ?? "",
            RiskBusinessCaseId = t.RiskBusinessCaseId ?? "",
            ChannelId = t.ChannelId,
            LoanApplicationAssessmentId = t.LoanApplicationAssessmentId,
            RiskSegment = t.RiskSegment,
            Created = new CIS.Infrastructure.gRPC.CisTypes.ModificationStamp(t.CreatedUserId, t.CreatedUserName, t.CreatedTime)
        };
    }
}
