using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVersions;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<DocumentTemplateVersionItem>> DocumentTemplateVersions(DocumentTemplateVersionsRequest request, CallContext context = default);
    }
}