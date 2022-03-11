using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> PropertySettlement(Endpoints.PropertySettlement.PropertySettlementRequest request, CallContext context = default);
    }
}