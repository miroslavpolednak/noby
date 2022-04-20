using DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<RiskApplicationTypeItem>> RiskApplicationTypes(RiskApplicationTypesRequest request, CallContext context = default);
    }
}
