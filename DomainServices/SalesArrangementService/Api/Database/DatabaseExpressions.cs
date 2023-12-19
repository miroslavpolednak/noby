﻿using System.Linq.Expressions;

namespace DomainServices.SalesArrangementService.Api.Database;

internal static class DatabaseExpressions
{
    public static Expression<Func<Entities.SalesArrangement, Contracts.SalesArrangement>> SalesArrangementDetail()
    {
        return t => new Contracts.SalesArrangement
        {
            OfferDocumentId = t.OfferDocumentId,
            PcpId = t.PcpId,
            SalesArrangementId = t.SalesArrangementId,
            CaseId = t.CaseId,
            State = t.State,
            OfferId = t.OfferId,
            SalesArrangementTypeId = t.SalesArrangementTypeId,
            ContractNumber = t.ContractNumber ?? "",
            RiskBusinessCaseId = t.RiskBusinessCaseId ?? "",
            ChannelId = t.ChannelId,
            LoanApplicationAssessmentId = t.LoanApplicationAssessmentId ?? "",
            RiskSegment = t.RiskSegment ?? "",
            CommandId = t.CommandId ?? "",
            Created = new SharedTypes.GrpcTypes.ModificationStamp(t.CreatedUserId, t.CreatedUserName, t.CreatedTime),
            OfferGuaranteeDateFrom = t.OfferGuaranteeDateFrom,
            OfferGuaranteeDateTo = t.OfferGuaranteeDateTo,
            RiskBusinessCaseExpirationDate = t.RiskBusinessCaseExpirationDate,
            FirstSignatureDate = t.FirstSignatureDate
        };
    }
}
