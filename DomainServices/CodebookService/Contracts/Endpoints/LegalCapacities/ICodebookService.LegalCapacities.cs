using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItemWithCode>> LegalCapacities(Endpoints.LegalCapacities.LegalCapacitiesRequest request, CallContext context = default);
    }
}