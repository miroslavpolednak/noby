using DomainServices.CodebookService.Contracts.Endpoints.LegalCapacities;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<LegalCapacityItem>> LegalCapacities(LegalCapacitiesRequest request, CallContext context = default);
    }
}