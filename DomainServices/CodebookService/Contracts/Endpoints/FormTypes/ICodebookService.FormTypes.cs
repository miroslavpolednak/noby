using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.FormTypes.FormTypeItem>> FormTypes(Endpoints.FormTypes.FormTypesRequest request, CallContext context = default);
    }
}