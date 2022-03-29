using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> LegalCapacities(Endpoints.IncomeTypes.IncomeTypesRequest request, CallContext context = default);
    }
}