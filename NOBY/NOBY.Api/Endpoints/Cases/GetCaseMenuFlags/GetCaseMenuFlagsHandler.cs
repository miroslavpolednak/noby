using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.ProductService.Clients;
using NOBY.Dto.Documents;
using NOBY.Services.DocumentHelper;

namespace NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;

internal sealed class GetCaseMenuFlagsHandler 
    : IRequestHandler<GetCaseMenuFlagsRequest, GetCaseMenuFlagsResponse>
{
    public async Task<GetCaseMenuFlagsResponse> Handle(GetCaseMenuFlagsRequest request, CancellationToken cancellationToken)
    {
        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest 
        { 
            CaseId = request.CaseId
        };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange(new List<int> { 100, 110, 200, 300 });
        var documentsInQueue = await _documentArchiveServiceClient.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        return new GetCaseMenuFlagsResponse
        {
            ParametersMenuItem = new(),
            DebtorsItem = new(),
            TasksMenuItem = new(),
            ChangeRequestsMenuItem = new()
            {
                IsActive = _currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access)
            },
            DocumentsMenuItem = await getDocuments(request.CaseId, documentsInQueue, cancellationToken),
            CovenantsMenuItem = await getCovenants(request.CaseId, documentsInQueue, cancellationToken),
            RealEstatesMenuItem = await getRealEstates(request.CaseId, cancellationToken)
        };
    }

    private async Task<GetCaseMenuFlagsItem> getRealEstates(long caseId, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.ValidateCaseId(caseId, false, cancellationToken);

        return new GetCaseMenuFlagsItem
        {
            Flag = GetCaseMenuFlagsTypes.NoFlag,
            IsActive = !(!_currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access) || caseInstance.State == 1)
        };
    }

    private async Task<GetCaseMenuFlagsItem> getCovenants(long caseId, GetDocumentsInQueueResponse documentsInQueue, CancellationToken cancellationToken)
    {
        var response = new GetCaseMenuFlagsItem
        {
            IsActive = false
        };

        try
        {
            var productInstance = await _productService.GetMortgage(caseId, cancellationToken);
            response.IsActive = productInstance.Mortgage?.ContractSignedDate != null;
        }
        catch { } // je v poradku, ze toto nekdy spadne - produkt nemusi byt v KonsDb

        if (_currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access))
        {
            if (documentsInQueue.QueuedDocuments.Any(t => t.StatusInQueue == 300))
            {
                response.Flag = GetCaseMenuFlagsTypes.ExclamationMark;
            }
            else if (documentsInQueue.QueuedDocuments.Any(t => (new[] { 100, 110, 200 }).Contains(t.StatusInQueue)))
            {
                response.Flag = GetCaseMenuFlagsTypes.InProcessing;
            }
        }

        return response;
    }

    private async Task<GetCaseMenuFlagsItem> getDocuments(long caseId, GetDocumentsInQueueResponse documentsInQueue, CancellationToken cancellationToken)
    {
        var getDocumentsInQueueMetadata = _documentHelper.MapGetDocumentsInQueueMetadata(documentsInQueue);
        var documentsInQueueFiltered = await _documentHelper.FilterDocumentsVisibleForKb(getDocumentsInQueueMetadata, cancellationToken);

        return new()
        {
            Flag = getDocumentsFlag(documentsInQueueFiltered.ToList()),
            IsActive = true
        };
    }

    private static GetCaseMenuFlagsTypes getDocumentsFlag(IEnumerable<DocumentsMetadata> documentsInQueueFiltered)
    {
        if (documentsInQueueFiltered.Any(s => s.UploadStatus == UploadStatuses.Error))
        {
            return GetCaseMenuFlagsTypes.ExclamationMark;
        }
        else if (documentsInQueueFiltered.Any(s => s.UploadStatus == UploadStatuses.InProgress))
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

    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IProductServiceClient _productService;
    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    private readonly IDocumentHelperService _documentHelper;
    private readonly ICaseServiceClient _caseService;

    public GetCaseMenuFlagsHandler(
        ICurrentUserAccessor currentUserAccessor,
        ICaseServiceClient caseService,
        IProductServiceClient productService,
        IDocumentArchiveServiceClient documentArchiveServiceClient,
        IDocumentHelperService documentHelper)
    {
        _currentUserAccessor = currentUserAccessor;
        _caseService = caseService;
        _productService = productService;
        _documentArchiveServiceClient = documentArchiveServiceClient;
        _documentHelper = documentHelper;
    }
}

