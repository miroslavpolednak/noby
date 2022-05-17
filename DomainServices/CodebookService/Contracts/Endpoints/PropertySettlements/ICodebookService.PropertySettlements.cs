using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.PropertySettlements.PropertySettlementItem>> PropertySettlements(Endpoints.PropertySettlements.PropertySettlementsRequest request, CallContext context = default);
    }
}