using DomainServices.DocumentOnSAService.Clients;
using NOBY.Services.EaCodeMain;

namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchDocumentsOnSaHandler : IRequestHandler<DocumentOnSaSearchDocumentsOnSaRequest, DocumentOnSaSearchDocumentsOnSaResponse>
{
    private readonly Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IEaCodeMainHelper _eaCodeMainHelper;

    public SearchDocumentsOnSaHandler(
        IDocumentOnSAServiceClient documentOnSAService,
        IEaCodeMainHelper eaCodeMainHelper,
        Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService salesArrangementAuthorization)
    {
        _documentOnSAService = documentOnSAService;
        _eaCodeMainHelper = eaCodeMainHelper;
        _salesArrangementAuthorization = salesArrangementAuthorization;
    }

    public async Task<DocumentOnSaSearchDocumentsOnSaResponse> Handle(DocumentOnSaSearchDocumentsOnSaRequest request, CancellationToken cancellationToken)
    {
        // validace prav
        await _salesArrangementAuthorization.ValidateSaAccessBySaType213And248BySAId(request.SalesArrangementId, cancellationToken);

        await _eaCodeMainHelper.ValidateEaCodeMain(request.EaCodeMainId!.Value, cancellationToken);

        var documentsOnSaResponse = await _documentOnSAService.GetDocumentsOnSAList(request.SalesArrangementId, cancellationToken);

        if (documentsOnSaResponse.DocumentsOnSA.Count == 0)
        {
            throw new CisNotFoundException(90100, $"No items found for SalesArrangement {request.SalesArrangementId}");
        }

        var documentTypesForEaCodeMain = await _eaCodeMainHelper.GetDocumentTypeIdsAccordingEaCodeMain(request.EaCodeMainId.Value, cancellationToken);

        if (documentTypesForEaCodeMain.Count == 0)
        {
            return new DocumentOnSaSearchDocumentsOnSaResponse { FormIds = [] };
        }

        var documentsOnSaFiltered = documentsOnSaResponse.DocumentsOnSA
                .Where(f => documentTypesForEaCodeMain.Contains(f.DocumentTypeId!.Value)
                            && !string.IsNullOrWhiteSpace(f.FormId)
                            && !f.IsFinal
                            && f.IsSigned
                            && f.SignatureTypeId == (int)SignatureTypes.Paper);

        return new DocumentOnSaSearchDocumentsOnSaResponse
        {
            FormIds = documentsOnSaFiltered.Select(s => new SharedTypesSearchResponseItem
            {
                FormId = s.FormId
            }).ToList()
        };
    }
}
