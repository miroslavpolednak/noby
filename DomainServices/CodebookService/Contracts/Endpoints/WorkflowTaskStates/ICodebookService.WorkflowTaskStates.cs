using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStates;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<WorkflowTaskStateItem>> WorkflowTaskStates(WorkflowTaskStatesRequest request, CallContext context = default);
    }
}
