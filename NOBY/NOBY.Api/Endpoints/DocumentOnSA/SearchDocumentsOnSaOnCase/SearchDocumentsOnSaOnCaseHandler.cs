using DomainServices.DocumentOnSAService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Dto.Signing;
using NOBY.Services.EaCodeMain;

namespace NOBY.Api.Endpoints.DocumentOnSA.SearchDocumentsOnSaOnCase;

public class SearchDocumentsOnSaOnCaseHandler : IRequestHandler<SearchDocumentsOnSaOnCaseRequest, SearchDocumentsOnSaOnCaseResponse>
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService;

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IEaCodeMainHelper _eaCodeMainHelper;

    public SearchDocumentsOnSaOnCaseHandler(
        IDocumentOnSAServiceClient documentOnSAService,
        ISalesArrangementServiceClient salesArrangementService,
        IEaCodeMainHelper eaCodeMainHelper
        )
    {
        _documentOnSAService = documentOnSAService;

        _salesArrangementService = salesArrangementService;
        _eaCodeMainHelper = eaCodeMainHelper;
    }

    public async Task<SearchDocumentsOnSaOnCaseResponse> Handle(SearchDocumentsOnSaOnCaseRequest request, CancellationToken cancellationToken)
    {
        await _eaCodeMainHelper.ValidateEaCodeMain(request.EACodeMainId!.Value, cancellationToken);

        var saResponse = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);

        var formIds = new List<string>();
        foreach (var salesArragment in saResponse.SalesArrangements)
        {
            var documentsOnSaResponse = await _documentOnSAService.GetDocumentsOnSAList(salesArragment.SalesArrangementId, cancellationToken);
            var documentTypesForEaCodeMain = await _eaCodeMainHelper.GetDocumentTypeIdsAccordingEaCodeMain(request.EACodeMainId!.Value, cancellationToken);

            var documentsOnSaFiltered = documentsOnSaResponse.DocumentsOnSA
               .Where(f => documentTypesForEaCodeMain.Contains(f.DocumentTypeId!.Value)
                           && !string.IsNullOrWhiteSpace(f.FormId)
                           && !f.IsFinal
                           && f.IsSigned);

            formIds.AddRange(documentsOnSaFiltered.Select(d => d.FormId));
        }

        return new SearchDocumentsOnSaOnCaseResponse()
        {
            FormIds = formIds.ConvertAll(r => new SearchResponseItem { FormId = r })
        };
    }
}
