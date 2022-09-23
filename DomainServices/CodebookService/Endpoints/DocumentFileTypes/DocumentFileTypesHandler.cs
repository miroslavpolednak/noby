using DomainServices.CodebookService.Contracts.Endpoints.DocumentFileTypes;

namespace DomainServices.CodebookService.Endpoints.DocumentFileTypes;

internal class DocumentFileTypesHandler
    : IRequestHandler<DocumentFileTypesRequest, List<DocumentFileTypeItem>>
{
    public Task<List<DocumentFileTypeItem>> Handle(DocumentFileTypesRequest request, CancellationToken cancellationToken)
    {
        var printableFileTypeIds = new int[] { 1, 2 };

        //TODO nakesovat?
        var values = FastEnum.GetValues<CIS.Foms.Enums.DocumentFileType>()
            .Select(t => new DocumentFileTypeItem()
            {
                Id = (int)t,
                EnumValue = t,
                DocumenFileType = t.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name ?? String.Empty,
                IsPrintingSupported = printableFileTypeIds.Contains((int)t),
            })
            .ToList();

        return Task.FromResult(values);
    }
}
