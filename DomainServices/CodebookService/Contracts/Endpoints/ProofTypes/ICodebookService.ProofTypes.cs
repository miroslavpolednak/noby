using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.ProofTypes.ProofTypeItem>> ProofTypes(Endpoints.ProofTypes.ProofTypesRequest request, CallContext context = default);
    }
}