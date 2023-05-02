using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<GenericCodebookItem>> WorkflowTaskConsultationTypes(Endpoints.WorkflowTaskConsultationTypes.WorkflowTaskConsultationTypesRequest request, CallContext context = default);
}