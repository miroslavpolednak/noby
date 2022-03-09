using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> ObligationTypes(Endpoints.ObligationTypes.ObligationTypesRequest request, CallContext context = default);
    }
}