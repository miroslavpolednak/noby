using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<DocumentTemplateTypeItem>> DocumentTemplateTypes(DocumentTemplateTypesRequest request, CallContext context = default);
    }
}