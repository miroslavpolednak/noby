using DomainServices.CodebookService.Contracts.Endpoints.RealEstatePurchaseTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<RealEstatePurchaseTypeItem>> RealEstatePurchaseTypes(RealEstatePurchaseTypesRequest request, CallContext context = default);
}