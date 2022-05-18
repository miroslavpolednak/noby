using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> WorkflowTaskCategories(Endpoints.WorkflowTaskCategories.WorkflowTaskCategoriesRequest request, CallContext context = default);
    }
}