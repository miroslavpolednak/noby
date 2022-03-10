using DomainServices.CodebookService.Contracts.Endpoints.RealEstateTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<RealEstateTypeItem>> RealEstateTypes(RealEstateTypesRequest request, CallContext context = default);
}