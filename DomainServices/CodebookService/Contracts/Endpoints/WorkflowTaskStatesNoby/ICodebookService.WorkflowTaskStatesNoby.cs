using DomainServices.CodebookService.Contracts.Endpoints.WorkflowTaskStatesNoby;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;
public partial interface ICodebookService
{
    [OperationContract]
    Task<List<WorkflowTaskStateNobyItem>> WorkflowTaskStatesNoby(WorkflowTaskStatesNobyRequest request, CallContext context = default);
}