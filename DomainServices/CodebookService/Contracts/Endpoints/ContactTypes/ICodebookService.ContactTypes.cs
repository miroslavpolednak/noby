using DomainServices.CodebookService.Contracts.Endpoints.ContactTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<ContactTypeItem>> ContactTypes(ContactTypesRequest request, CallContext context = default);
}