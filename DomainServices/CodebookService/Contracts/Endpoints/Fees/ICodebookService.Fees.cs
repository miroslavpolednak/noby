using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.Fees.FeeItem>> Fees(Endpoints.Fees.FeesRequest request, CallContext context = default);
    }
}