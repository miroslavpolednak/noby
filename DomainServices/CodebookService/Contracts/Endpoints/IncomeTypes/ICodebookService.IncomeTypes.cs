using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItemWithCode>> IncomeTypes(Endpoints.IncomeTypes.IncomeTypesRequest request, CallContext context = default);
    }
}