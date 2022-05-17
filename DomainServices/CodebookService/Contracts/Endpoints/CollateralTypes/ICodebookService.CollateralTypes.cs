using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.CollateralTypes.CollateralTypeItem>> CollateralTypes(Endpoints.CollateralTypes.CollateralTypesRequest request, CallContext context = default);
    }
}