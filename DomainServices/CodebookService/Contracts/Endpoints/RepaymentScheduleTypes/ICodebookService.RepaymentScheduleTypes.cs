using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.RepaymentScheduleTypes.RepaymentScheduleTypeItem>> RepaymentScheduleTypes(Endpoints.RepaymentScheduleTypes.RepaymentScheduleTypesRequest request, CallContext context = default);
    }
}