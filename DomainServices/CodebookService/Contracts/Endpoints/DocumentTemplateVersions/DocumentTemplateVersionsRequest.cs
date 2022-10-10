namespace DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVersions;

[DataContract]
public class DocumentTemplateVersionsRequest : IRequest<List<DocumentTemplateVersionItem>>
{
}