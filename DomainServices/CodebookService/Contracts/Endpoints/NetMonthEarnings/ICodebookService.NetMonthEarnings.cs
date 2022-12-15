using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.NetMonthEarnings.NetMonthEarningItem>> NetMonthEarnings(Endpoints.NetMonthEarnings.NetMonthEarningsRequest request, CallContext context = default);
    }
}
