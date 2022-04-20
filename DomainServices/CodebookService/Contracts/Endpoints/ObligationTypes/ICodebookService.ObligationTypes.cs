using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItemWithCode>> ObligationTypes(Endpoints.ObligationTypes.ObligationTypesRequest request, CallContext context = default);
    }
}