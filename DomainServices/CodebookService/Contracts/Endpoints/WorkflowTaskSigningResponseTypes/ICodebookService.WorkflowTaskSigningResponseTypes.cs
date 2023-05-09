using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<GenericCodebookItem>> WorkflowTaskSigningResponseTypes(Endpoints.WorkflowTaskSigningResponseTypes.WorkflowTaskSigningResponseTypesRequest request, CallContext context = default);
}