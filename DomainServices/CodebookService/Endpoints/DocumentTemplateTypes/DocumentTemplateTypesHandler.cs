using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateTypes;

namespace DomainServices.CodebookService.Endpoints.DocumentTemplateTypes;

internal class DocumentTemplateTypesHandler
    : IRequestHandler<DocumentTemplateTypesRequest, List<DocumentTemplateTypeItem>>
{
    public Task<List<DocumentTemplateTypeItem>> Handle(DocumentTemplateTypesRequest request, CancellationToken cancellationToken)
    {
        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.DocumentTemplateType>()
            .Select(t => new DocumentTemplateTypeItem()
            {
                Id = (int)t,
                EnumValue = t,
                Name = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? "",
                ShortName = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.ShortName ?? ""
            })
            .ToList();

        return Task.FromResult(values);
    }
}