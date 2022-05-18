using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> WorkflowTaskStates(Endpoints.WorkflowTaskStates.WorkflowTaskStatesRequest request, CallContext context = default);
    }
}
