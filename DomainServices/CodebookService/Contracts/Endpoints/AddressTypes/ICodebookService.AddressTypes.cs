using ProtoBuf.Grpc;
using DomainServices.CodebookService.Contracts.Endpoints.AddressTypes;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<AddressTypeItem>> AddressTypes(AddressTypesRequest request, CallContext context = default);
}
