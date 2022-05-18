using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.WorkflowTaskTypes.WorkflowTaskTypeItem>> WorkflowTaskTypes(Endpoints.WorkflowTaskTypes.WorkflowTaskTypesRequest request, CallContext context = default);
    }
}