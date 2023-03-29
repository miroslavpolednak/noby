using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVariants;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;
public partial interface ICodebookService
{
    [OperationContract]
    Task<List<DocumentTemplateVariantItem>> DocumentTemplateVariants(DocumentTemplateVariantsRequest request, CallContext context = default);
}