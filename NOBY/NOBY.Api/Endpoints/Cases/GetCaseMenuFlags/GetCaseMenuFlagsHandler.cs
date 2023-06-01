using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.ProductService.Clients;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using NOBY.Api.Endpoints.Shared;

namespace NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;

internal sealed class GetCaseMenuFlagsHandler : IRequestHandler<GetCaseMenuFlagsRequest, GetCaseMenuFlagsResponse>
{
    private readonly IProductServiceClient _productService;
    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    private readonly IDocumentHelper _documentHelper;

    public GetCaseMenuFlagsHandler(
            IProductServiceClient productService,
            IDocumentArchiveServiceClient documentArchiveServiceClient,
            IDocumentHelper documentHelper
            )
    {
        _productService = productService;
        _documentArchiveServiceClient = documentArchiveServiceClient;
        _documentHelper = documentHelper;
    }

    public async Task<GetCaseMenuFlagsResponse> Handle(GetCaseMenuFlagsRequest request, CancellationToken cancellationToken)
    {
        return new GetCaseMenuFlagsResponse
        {
            DocumentsMenuItem = await getDocuments(request.CaseId, cancellationToken),
            CovenantsMenuItem = await getCovenants(request.CaseId, cancellationToken)
        };
    }

    private async Task<GetCaseMenuFlagsItem> getCovenants(long caseId, CancellationToken cancellationToken)
    {
        var productInstance = await _productService.GetMortgage(caseId, cancellationToken);

        return new GetCaseMenuFlagsItem
        {
            Flag = GetCaseMenuFlagsTypes.NoFlag,
            IsActive = productInstance.Mortgage?.ContractSignedDate != null
        };
    }

    private async Task<GetCaseMenuFlagsItem> getDocuments(long caseId, CancellationToken cancellationToken)
    {
        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest { CaseId = caseId };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange(new List<int> { 100, 110, 200, 300 });
        var getDocumentsInQueueResult = await _documentArchiveServiceClient.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        var getDocumentsInQueueMetadata = _documentHelper.MapGetDocumentsInQueueMetadata(getDocumentsInQueueResult);
        var documentsInQueueFiltered = await _documentHelper.FilterDocumentsVisibleForKb(getDocumentsInQueueMetadata, cancellationToken);

        return new()
        {
            Flag = getDocumentsFlag(documentsInQueueFiltered.ToList()),
            IsActive = true
        };
    }

    private static GetCaseMenuFlagsTypes getDocumentsFlag(IEnumerable<DocumentsMetadata> documentsInQueueFiltered)
    {
        if (documentsInQueueFiltered.Any(s => s.UploadStatus == UploadStatus.Error))
        {
            return GetCaseMenuFlagsTypes.ExclamationMark;
        }
        else if (documentsInQueueFiltered.Any(s => s.UploadStatus == UploadStatus.InProgress))
        {
            return GetCaseMenuFlagsTypes.InProcessing;
        }
        else if (!documentsInQueueFiltered.Any())
        {
            return GetCaseMenuFlagsTypes.NoFlag;
        }
        else
        {
            throw new ArgumentException("This state isn't supported");
        }
    }
}

