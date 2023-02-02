using DomainServices.CodebookService.Contracts.Endpoints.IdentificationSubjectMethods;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<GenericCodebookItem>> IdentificationSubjectMethods(IdentificationSubjectMethodsRequest request, CallContext context = default);
}
