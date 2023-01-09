using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<GenericCodebookItemWithCode>> IncomeForeignTypes(Endpoints.IncomeForeignTypes.IncomeForeignTypesRequest request, CallContext context = default);
}