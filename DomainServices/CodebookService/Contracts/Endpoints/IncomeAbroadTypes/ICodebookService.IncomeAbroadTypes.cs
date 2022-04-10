using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> IncomeAbroadTypes(Endpoints.IncomeAbroadTypes.IncomeAbroadTypesRequest request, CallContext context = default);
    }
}