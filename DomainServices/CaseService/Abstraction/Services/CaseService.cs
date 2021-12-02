using CIS.Core.Results;
using CIS.Core.Types;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Abstraction.Services;

internal class CaseService : ICaseServiceAbstraction
{
    public Task<IServiceCallResult> CreateCase(CreateCaseRequest model)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> GetCaseDetail(long caseId)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> GetCaseList(int? partyId, int? state, PaginableRequest pagination)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> LinkOwnerToCase(long caseId, int partyId)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> UpdateCaseCustomer(UpdateCaseCustomerRequest model)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> UpdateCaseData(long caseId, string contractNumber)
    {
        throw new NotImplementedException();
    }

    public Task<IServiceCallResult> UpdateCaseState(long caseId, int state)
    {
        throw new NotImplementedException();
    }
}
