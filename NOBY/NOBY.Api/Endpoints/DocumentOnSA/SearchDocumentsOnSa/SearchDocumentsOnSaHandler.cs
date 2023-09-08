using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using NOBY.Dto.Signing;
using NOBY.Services.EaCodeMain;

namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchDocumentsOnSaHandler : IRequestHandler<SearchDocumentsOnSaRequest, SearchDocumentsOnSaResponse>
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IEaCodeMainHelper _eaCodeMainHelper;

    public SearchDocumentsOnSaHandler(
        IDocumentOnSAServiceClient documentOnSAService,
        IEaCodeMainHelper eaCodeMainHelper)
    {
        _documentOnSAService = documentOnSAService;
        _eaCodeMainHelper = eaCodeMainHelper;
    }

    public async Task<SearchDocumentsOnSaResponse> Handle(SearchDocumentsOnSaRequest request, CancellationToken cancellationToken)
    {
        await _eaCodeMainHelper.ValidateEaCodeMain(request.EACodeMainId!.Value, cancellationToken);

        var documentsOnSaResponse = await _documentOnSAService.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);

        if (!documentsOnSaResponse.DocumentsOnSA.Any())
        {
            throw new CisNotFoundException(90100, $"No items found for SalesArrangement {request.SalesArrangementId}");
        }

        var documentTypesForEaCodeMain = await _eaCodeMainHelper.GetDocumentTypeIdsAccordingEaCodeMain(request.EACodeMainId.Value, cancellationToken);

        if (!documentTypesForEaCodeMain.Any())
        {
            return new SearchDocumentsOnSaResponse { FormIds = Array.Empty<SearchResponseItem>() };
        }

        var documentsOnSaFiltered = documentsOnSaResponse.DocumentsOnSA
                .Where(f => documentTypesForEaCodeMain.Contains(f.DocumentTypeId!.Value)
                            && !string.IsNullOrWhiteSpace(f.FormId)
                            && !f.IsFinal
                            && f.IsSigned);

        return new SearchDocumentsOnSaResponse
        {
            FormIds = documentsOnSaFiltered.Select(s => new SearchResponseItem
            {
                FormId = s.FormId
            }).ToList()
        };
    }
}
