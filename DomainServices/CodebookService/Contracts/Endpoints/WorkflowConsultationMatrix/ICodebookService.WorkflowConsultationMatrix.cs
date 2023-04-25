using DomainServices.CodebookService.Contracts.Endpoints.WorkflowConsultationMatrix;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<WorkflowConsultationMatrixItem>> WorkflowConsultationMatrix(WorkflowConsultationMatrixRequest request, CallContext context = default);
}
