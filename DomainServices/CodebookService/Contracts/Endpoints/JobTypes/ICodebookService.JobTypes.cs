using DomainServices.CodebookService.Contracts.Endpoints.JobTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<JobTypeItem>> JobTypes(JobTypesRequest request, CallContext context = default);
}