using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;

namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchHandler : IRequestHandler<SearchRequest, SearchResponse>
{
    private readonly IDocumentOnSAServiceClient _client;
    private readonly ICodebookServiceClient _codebookServiceClients;

    public SearchHandler(
        IDocumentOnSAServiceClient client,
        ICodebookServiceClient codebookServiceClients)
    {
        _client = client;
        _codebookServiceClients = codebookServiceClients;
    }

    public async Task<SearchResponse> Handle(SearchRequest request, CancellationToken cancellationToken)
    {
        var documentsOnSa = await _client.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);

        if (!documentsOnSa.DocumentsOnSAToSign.Any())
        {
            throw new CisNotFoundException(90100, $"No items found for SalesArrangement {request.SalesArrangementId}");
        }

        var documentTypes = await _codebookServiceClients.DocumentTypes(cancellationToken);
        var documentTypeFiltered = documentTypes.FirstOrDefault(r => r.EACodeMainId == request.EACodeMainId && r.IsFormIdRequested);

        if (documentTypeFiltered is null)
        {
            return new SearchResponse { FormIds = Array.Empty<SearchResponseItem>() };
        }
        
        var documentsOnSaFiltered = documentsOnSa.DocumentsOnSAToSign
                .Where(f => f.DocumentTypeId == documentTypeFiltered.Id 
                            && !string.IsNullOrWhiteSpace(f.FormId)
                            && f.IsDocumentArchived == false);

        return new SearchResponse
        {
            FormIds = documentsOnSaFiltered.Select(s => new SearchResponseItem
            {
                FormId = s.FormId
            }).ToList()
        };
    }
}
