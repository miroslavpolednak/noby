using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.ProductService.Clients;
using NOBY.Services.DocumentHelper;

namespace NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;

internal sealed class GetCaseMenuFlagsHandler(
    ICurrentUserAccessor _currentUserAccessor,
    ICaseServiceClient _caseService,
    IProductServiceClient _productService,
    IDocumentArchiveServiceClient _documentArchiveServiceClient,
    IDocumentHelperService _documentHelper)
        : IRequestHandler<GetCaseMenuFlagsRequest, GetCaseMenuFlagsResponse>
{
    public async Task<GetCaseMenuFlagsResponse> Handle(GetCaseMenuFlagsRequest request, CancellationToken cancellationToken)
    {
        // instance case
        var caseInstance = await _caseService.ValidateCaseId(request.CaseId, false, cancellationToken);

        // seznam dokumentu
        var getDocumentsInQueueRequest = new GetDocumentsInQueueRequest
        {
            CaseId = request.CaseId
        };
        getDocumentsInQueueRequest.StatusesInQueue.AddRange(_values);
        var documentsInQueue = await _documentArchiveServiceClient.GetDocumentsInQueue(getDocumentsInQueueRequest, cancellationToken);

        return new GetCaseMenuFlagsResponse
        {
            ParametersMenuItem = new(),
            DebtorsItem = new(),
            TasksMenuItem = new(),
            ChangeRequestsMenuItem = new GetCaseMenuFlagsItem
            {
                IsActive = _currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access) && caseInstance.State != (int)CaseStates.InProgress && caseInstance.State != (int)CaseStates.ToBeCancelled
            },
            RealEstatesMenuItem = new GetCaseMenuFlagsItem
            {
                Flag = GetCaseMenuFlagsTypes.NoFlag,
                IsActive = _currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access) && caseInstance.State != (int)CaseStates.InProgress && caseInstance.State != (int)CaseStates.ToBeCancelled
            },
            DocumentsMenuItem = await getDocuments(documentsInQueue, cancellationToken),
            CovenantsMenuItem = await getCovenants(request.CaseId, cancellationToken),
            RefinancingMenuItem = new()
            {
                IsActive = _currentUserAccessor.HasPermission(UserPermissions.REFINANCING_Manage) && (caseInstance.State is (int)CaseStates.InDisbursement or (int)CaseStates.InAdministration)
            }
        };
    }

    private async Task<GetCaseMenuFlagsItem> getCovenants(long caseId, CancellationToken cancellationToken)
    {
        var response = new GetCaseMenuFlagsItem();

        if (_currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access))
        {
            try
            {
                var productInstance = await _productService.GetMortgage(caseId, cancellationToken);
                response.IsActive = !string.IsNullOrEmpty(productInstance.Mortgage?.PaymentAccount?.Number);
            }
            catch // je v poradku, ze toto nekdy spadne - produkt nemusi byt v KonsDb
            {
                response.IsActive = false;
            }
        }
        else
        {
            response.IsActive = false;
        }

        return response;
    }

    private async Task<GetCaseMenuFlagsItem> getDocuments(GetDocumentsInQueueResponse documentsInQueue, CancellationToken cancellationToken)
    {
        var getDocumentsInQueueMetadata = _documentHelper.MapGetDocumentsInQueueMetadata(documentsInQueue);
        var documentsInQueueFiltered = await _documentHelper.FilterDocumentsVisibleForKb(getDocumentsInQueueMetadata, cancellationToken);

        var response = new GetCaseMenuFlagsItem();

        if (_currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access))
        {
            if (documentsInQueue.QueuedDocuments.Any(t => t.StatusInQueue == 300))
            {
                response.Flag = GetCaseMenuFlagsTypes.ExclamationMark;
            }
            else if (documentsInQueue.QueuedDocuments.Any(t => _requiredStatusesInDocumentQueue.Contains(t.StatusInQueue)))
            {
                response.Flag = GetCaseMenuFlagsTypes.InProcessing;
            }
        }

        return response;
    }

    private static readonly int[] _requiredStatusesInDocumentQueue = [100, 110, 200];
    private static readonly int[] _values = [ 100, 110, 200, 300 ];
}

