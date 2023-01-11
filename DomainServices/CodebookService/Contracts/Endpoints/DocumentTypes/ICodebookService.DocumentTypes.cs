using DomainServices.CodebookService.Contracts.Endpoints.DocumentTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<DocumentTypeItem>> DocumentTypes(DocumentTypesRequest request, CallContext context = default);
}