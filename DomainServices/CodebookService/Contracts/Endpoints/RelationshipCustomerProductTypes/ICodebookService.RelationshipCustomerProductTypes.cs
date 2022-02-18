using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.RelationshipCustomerProductTypes.RelationshipCustomerProductTypeItem>> RelationshipCustomerProductTypes(Endpoints.RelationshipCustomerProductTypes.RelationshipCustomerProductTypesRequest request, CallContext context = default);
    }
}
