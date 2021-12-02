using CIS.Core.Results;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Abstraction;

public interface ICaseServiceAbstraction
{
    Task<IServiceCallResult> CreateCase(CreateCaseRequest model);
    
    Task<IServiceCallResult> GetCaseDetail(long caseId);
    
    Task<IServiceCallResult> GetCaseList(int? partyId, int? state, CIS.Core.Types.PaginableRequest pagination);
    
    Task<IServiceCallResult> LinkOwnerToCase(long caseId, int partyId);
    
    Task<IServiceCallResult> UpdateCaseData(long caseId, string contractNumber);
    
    Task<IServiceCallResult> UpdateCaseState(long caseId, int state);
    
    Task<IServiceCallResult> UpdateCaseCustomer(UpdateCaseCustomerRequest model);
}
