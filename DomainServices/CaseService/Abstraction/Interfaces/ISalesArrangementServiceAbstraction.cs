﻿using CIS.Core.Results;

namespace DomainServices.CaseService.Abstraction;

public interface ISalesArrangementServiceAbstraction
{
    Task<IServiceCallResult> CreateSalesArrangement(long caseId, int salesArrangementType, long? productInstanceId = null);

    Task<IServiceCallResult> GetSalesArrangementDetail(long salesArrangementId);
    
    Task<IServiceCallResult> GetSalesArrangementsByCaseId(long caseId, IEnumerable<int>? states);
    
    Task<IServiceCallResult> UpdateSalesArrangementState(long salesArrangementType, int state);
    
    Task<IServiceCallResult> ValidateSalesArrangement(long salesArrangementId);
    
    Task<IServiceCallResult> UpdateSalesArrangementData(long salesArrangementId);
}
