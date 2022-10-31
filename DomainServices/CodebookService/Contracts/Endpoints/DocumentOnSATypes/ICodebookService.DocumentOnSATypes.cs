using DomainServices.CodebookService.Contracts.Endpoints.DocumentOnSATypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<DocumentOnSATypeItem>> DocumentOnSATypes(DocumentOnSATypesRequest request, CallContext context = default);
    }
}