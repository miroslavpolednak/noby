using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;

namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchHandler : IRequestHandler<SearchRequest, SearchResponse>
{
    private readonly IDocumentOnSAServiceClient _client;
    private readonly ICodebookServiceClients _codebookServiceClients;

    public SearchHandler(
        IDocumentOnSAServiceClient client,
        ICodebookServiceClients codebookServiceClients)
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

        await ValidateEaCodeMain(request, cancellationToken);

        var documentTypes = await _codebookServiceClients.DocumentTypes(cancellationToken);
        var documentTypesFiltered = documentTypes.Where(r => r.EACodeMainId == request.EACodeMainId)
                                    .Select(s => s.Id)
                                    .ToList();

        if (!documentTypesFiltered.Any())
        {
            return new SearchResponse { FormIds = Array.Empty<SearchResponseItem>() };
        }

        var documentsOnSaFiltered = documentsOnSa.DocumentsOnSAToSign
                .Where(f => documentTypesFiltered.Contains(f.DocumentTypeId!.Value)
                            && !string.IsNullOrWhiteSpace(f.FormId)
                            && f.IsFinal == false 
                            && f.IsSigned == true);

        return new SearchResponse
        {
            FormIds = documentsOnSaFiltered.Select(s => new SearchResponseItem
            {
                FormId = s.FormId
            }).ToList()
        };
    }

    private async Task ValidateEaCodeMain(SearchRequest request, CancellationToken cancellationToken)
    {
        var eaCodeMains = await _codebookServiceClients.EaCodesMain(cancellationToken);
        var eaCodeMain = eaCodeMains.FirstOrDefault(r => r.Id == request.EACodeMainId);

        if (eaCodeMain is null)
            throw new NobyValidationException($"Specified EACodeMainId:{request.EACodeMainId} isn't valid");

        if (eaCodeMain.IsFormIdRequested == false)
            throw new NobyValidationException($"Specified EACodeMainId has IsFormIdRequested == false");
    }
}
