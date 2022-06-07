using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItemWithCode>> IncomeOtherTypes(Endpoints.IncomeOtherTypes.IncomeOtherTypesRequest request, CallContext context = default);
    }
}