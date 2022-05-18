using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.PaymentDays.PaymentDayItem>> PaymentDays(Endpoints.PaymentDays.PaymentDaysRequest request, CallContext context = default);
    }
}