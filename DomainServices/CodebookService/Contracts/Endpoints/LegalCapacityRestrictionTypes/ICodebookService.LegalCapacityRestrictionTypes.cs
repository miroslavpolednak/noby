using DomainServices.CodebookService.Contracts.Endpoints.LegalCapacityRestrictionTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<LegalCapacityRestrictionTypeItem>> LegalCapacityRestrictionTypes(LegalCapacityRestrictionTypesRequest request, CallContext context = default);
}