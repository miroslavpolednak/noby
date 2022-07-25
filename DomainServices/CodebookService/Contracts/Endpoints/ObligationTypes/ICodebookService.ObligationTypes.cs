using DomainServices.CodebookService.Contracts.Endpoints.ObligationTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<ObligationTypesItem>> ObligationTypes(ObligationTypesRequest request, CallContext context = default);
    }
}
