using DomainServices.CodebookService.Contracts.Endpoints.WorkflowProcessStatesNoby;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;
public partial interface ICodebookService
{
    [OperationContract]
    Task<List<WorkflowProcessStateNobyItem>> WorkflowProcessStatesNoby(WorkflowProcessStatesNobyRequest request, CallContext context = default);
}