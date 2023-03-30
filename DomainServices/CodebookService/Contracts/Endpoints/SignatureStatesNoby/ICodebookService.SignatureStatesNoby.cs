using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> SignatureStatesNoby(Endpoints.SignatureStatesNoby.SignatureStatesNobyRequest request, CallContext context = default);
    }
}
