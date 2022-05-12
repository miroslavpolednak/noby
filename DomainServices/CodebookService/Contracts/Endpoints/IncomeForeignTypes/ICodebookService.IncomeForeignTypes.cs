using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.IncomeForeignTypes.IncomeForeignTypeItem>> IncomeForeignTypes(Endpoints.IncomeForeignTypes.IncomeForeignTypesRequest request, CallContext context = default);
    }
}