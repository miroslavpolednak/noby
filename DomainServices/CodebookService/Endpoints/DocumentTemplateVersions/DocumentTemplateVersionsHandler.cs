using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVersions;
using DomainServices.CodebookService.Contracts.Endpoints.RepaymentScheduleTypes;

namespace DomainServices.CodebookService.Endpoints.DocumentTemplateVersions;

internal class DocumentTemplateVersionsHandler
    : IRequestHandler<DocumentTemplateVersionsRequest, List<DocumentTemplateVersionItem>>
{
    public Task<List<DocumentTemplateVersionItem>> Handle(DocumentTemplateVersionsRequest request, CancellationToken cancellationToken)
    {
        // TODO: Redirect to real data source!
        var ids = new List<int> { 1, 2, 3, 4, 5, 6 };
        return Task.FromResult(
            ids.Select(i => new DocumentTemplateVersionItem
            {
                Id = i,
                DocumentTemplateTypeId = i,
                DocumentVersion = "001A"
            }).ToList()
         );
    }
}