using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.GeneralDocument.GetGeneralDocuments;

internal sealed class GetGeneralDocumentsHandler : IRequestHandler<GetGeneralDocumentsRequest, List<GeneralDocumentGetGeneralDocumentsDocument>>
{
    public async Task<List<GeneralDocumentGetGeneralDocumentsDocument>> Handle(GetGeneralDocumentsRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        var documents = await _codebookService.GetGeneralDocumentList(cancellationToken);

        return documents.Select(d => new GeneralDocumentGetGeneralDocumentsDocument()
        {
            Id = d.Id,
            Filename = d.Filename,
            Format = d.Format,
            Name = d.Name
        }).ToList();
    }

    private readonly ICodebookServiceClient _codebookService;
    
    public GetGeneralDocumentsHandler(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }
}