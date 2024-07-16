using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Dto.Signing;
using NOBY.Services.EaCodeMain;

namespace NOBY.Api.Endpoints.DocumentOnSA.SearchDocumentsOnSaOnCase;

public class SearchDocumentsOnSaOnCaseHandler : IRequestHandler<DocumentOnSaSearchDocumentsOnSaOnCaseRequest, DocumentOnSaSearchDocumentsOnSaOnCaseResponse>
{
    private readonly Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IEaCodeMainHelper _eaCodeMainHelper;

    public SearchDocumentsOnSaOnCaseHandler(
        IDocumentOnSAServiceClient documentOnSAService,
        ISalesArrangementServiceClient salesArrangementService,
        IEaCodeMainHelper eaCodeMainHelper
,
        Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService salesArrangementAuthorization)
    {
        _documentOnSAService = documentOnSAService;

        _salesArrangementService = salesArrangementService;
        _eaCodeMainHelper = eaCodeMainHelper;
        _salesArrangementAuthorization = salesArrangementAuthorization;
    }

    public async Task<DocumentOnSaSearchDocumentsOnSaOnCaseResponse> Handle(DocumentOnSaSearchDocumentsOnSaOnCaseRequest request, CancellationToken cancellationToken)
    {
        await _eaCodeMainHelper.ValidateEaCodeMain(request.EaCodeMainId!.Value, cancellationToken);

        var saResponse = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);
        var salesArrangements = _salesArrangementAuthorization.FiltrSalesArrangements(saResponse.SalesArrangements);

        var formIds = new List<string>();
        foreach (var salesArragment in salesArrangements)
        {
            var documentsOnSaResponse = await _documentOnSAService.GetDocumentsOnSAList(salesArragment.SalesArrangementId, cancellationToken);
            var documentTypesForEaCodeMain = await _eaCodeMainHelper.GetDocumentTypeIdsAccordingEaCodeMain(request.EaCodeMainId!.Value, cancellationToken);

            var documentsOnSaFiltered = documentsOnSaResponse.DocumentsOnSA
               .Where(f => f.DocumentTypeId is not null
                           && documentTypesForEaCodeMain.Contains(f.DocumentTypeId!.Value)
                           && !string.IsNullOrWhiteSpace(f.FormId)
                           && !f.IsFinal
                           && f.IsSigned);
               
            formIds.AddRange(documentsOnSaFiltered.Select(d => d.FormId));
        }

        return new DocumentOnSaSearchDocumentsOnSaOnCaseResponse()
        {
            FormIds = formIds.ConvertAll(r => new SharedTypesSearchResponseItem { FormId = r })
        };
    }
}
