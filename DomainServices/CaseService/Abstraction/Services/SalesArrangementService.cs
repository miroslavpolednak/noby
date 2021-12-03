using CIS.Core.Results;
using CIS.Core.Types;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Abstraction.Services;

internal class SalesArrangementService : ISalesArrangementServiceAbstraction
{
    public Task<IServiceCallResult> CreateSalesArrangement(long caseId, int salesArrangementType, long? productInstanceId = null)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> GetSalesArrangementDetail(long salesArrangementId)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> GetSalesArrangementsByCaseId(long caseId, IEnumerable<int>? states)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> UpdateSalesArrangementData(long salesArrangementId)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> UpdateSalesArrangementState(long salesArrangementType, int state)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> ValidateSalesArrangement(long salesArrangementId)
    {
        throw new NotImplementedException();
    }
}
