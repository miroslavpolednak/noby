using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
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
    IDocumentHelperServiceOld _documentHelper)
        : IRequestHandler<GetCaseMenuFlagsRequest, CasesGetCaseMenuFlagsResponse>
{
    public async Task<CasesGetCaseMenuFlagsResponse> Handle(GetCaseMenuFlagsRequest request, CancellationToken cancellationToken)
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

        return new CasesGetCaseMenuFlagsResponse
        {
            ParametersMenuItem = new() { IsActive = true },
            DebtorsItem = new() { IsActive = true },
            TasksMenuItem = new() { IsActive = true },
            ChangeRequestsMenuItem = new CasesGetCaseMenuFlagsItem
            {
                IsActive = _currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access) 
                    && CaseHelpers.IsCaseInState(_caseStates1, (EnumCaseStates)caseInstance.State!)
            },
            RealEstatesMenuItem = new CasesGetCaseMenuFlagsItem
            {
                Flag = CasesGetCaseMenuFlagsItemFlag.NoFlag,
                IsActive = _currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access)
                    && CaseHelpers.IsCaseInState(_caseStates1, (EnumCaseStates)caseInstance.State!)
            },
            DocumentsMenuItem = await getDocuments(documentsInQueue, cancellationToken),
            CovenantsMenuItem = await getCovenants(request.CaseId, cancellationToken),
            RefinancingMenuItem = new()
            {
                IsActive = _currentUserAccessor.HasPermission(UserPermissions.REFINANCING_Manage) 
                    && (CaseHelpers.IsCaseInState([EnumCaseStates.InDisbursement, EnumCaseStates.InAdministration], (EnumCaseStates)caseInstance.State!))
            },
            ExtraPaymentMenuItem = new()
            {
                IsActive = _currentUserAccessor.HasPermission(UserPermissions.REFINANCING_Manage)
                    && (CaseHelpers.IsCaseInState([EnumCaseStates.InDisbursement, EnumCaseStates.InAdministration], (EnumCaseStates)caseInstance.State!))
            }
        };
    }

    private async Task<CasesGetCaseMenuFlagsItem> getCovenants(long caseId, CancellationToken cancellationToken)
    {
        var response = new CasesGetCaseMenuFlagsItem();

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

    private async Task<CasesGetCaseMenuFlagsItem> getDocuments(GetDocumentsInQueueResponse documentsInQueue, CancellationToken cancellationToken)
    {
        var getDocumentsInQueueMetadata = _documentHelper.MapGetDocumentsInQueueMetadata(documentsInQueue);
        var documentsInQueueFiltered = await _documentHelper.FilterDocumentsVisibleForKb(getDocumentsInQueueMetadata, cancellationToken);

        var response = new CasesGetCaseMenuFlagsItem()
        {
            IsActive = true
        };

        if (_currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access))
        {
            if (documentsInQueue.QueuedDocuments.Any(t => t.StatusInQueue == 300))
            {
                response.Flag = CasesGetCaseMenuFlagsItemFlag.ExclamationMark;
            }
            else if (documentsInQueue.QueuedDocuments.Any(t => _requiredStatusesInDocumentQueue.Contains(t.StatusInQueue)))
            {
                response.Flag = CasesGetCaseMenuFlagsItemFlag.InProcessing;
            }
        }

        return response;
    }

    private static readonly int[] _requiredStatusesInDocumentQueue = [100, 110, 200];
    private static readonly int[] _values = [ 100, 110, 200, 300 ];
    private static readonly EnumCaseStates[] _caseStates1 =
    [
        EnumCaseStates.InProcessing,
        EnumCaseStates.InSigning,
        EnumCaseStates.InDisbursement,
        EnumCaseStates.InAdministration,
        EnumCaseStates.Finished,
        EnumCaseStates.Cancelled,
        EnumCaseStates.InProcessingConfirmed,
        EnumCaseStates.ToBeCancelledConfirmed
    ];
}

