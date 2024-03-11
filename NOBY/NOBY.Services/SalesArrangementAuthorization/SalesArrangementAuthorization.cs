using CIS.Core.Security;
using NOBY.Infrastructure.Security;
using DomainServices.UserService.Clients.Authorization;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Services.SalesArrangementAuthorization;

[ScopedService, AsImplementedInterfacesService]
internal sealed class SalesArrangementAuthorizationService
    : ISalesArrangementAuthorizationService
{
    public List<SalesArrangement> FiltrSalesArrangements(IEnumerable<SalesArrangement> salesArrangements)
    {
        // refinancing
        if (!_currentUser.HasPermission(UserPermissions.SALES_ARRANGEMENT_RefinancingAccess))
        {
            salesArrangements = salesArrangements.Where(t => !ISalesArrangementAuthorizationService.RefinancingSATypes.Contains(t.SalesArrangementTypeId));
        }
        // ostatni sa
        if (!_currentUser.HasPermission(UserPermissions.SALES_ARRANGEMENT_Access))
        {
            salesArrangements = salesArrangements.Where(t => ISalesArrangementAuthorizationService.RefinancingSATypes.Contains(t.SalesArrangementTypeId));
        }

        return salesArrangements.ToList();
    }

    public async Task ValidateSaAccessBySaType213And248BySAId(int salesArrangementId, CancellationToken cancellationToken)
    {
        var sa = await _salesArrangementService.ValidateSalesArrangementId(salesArrangementId, true, cancellationToken);
        ValidateSaAccessBySaType213And248(sa.SalesArrangementTypeId!.Value);
    }

    public void ValidateSaAccessBySaType213And248(in int salesArrangementTypeId)
    {
        ValidateRefinancingPermissions(salesArrangementTypeId, UserPermissions.SALES_ARRANGEMENT_RefinancingAccess, UserPermissions.SALES_ARRANGEMENT_Access);
    }

    public async Task ValidateDocumentSigningMngBySaType237And246BySAId(int salesArrangementId, CancellationToken cancellationToken)
    {
        var sa = await _salesArrangementService.ValidateSalesArrangementId(salesArrangementId, true, cancellationToken);
        ValidateDocumentSigningMngBySaType237And246(sa.SalesArrangementTypeId!.Value);
    }

    public void ValidateDocumentSigningMngBySaType237And246(in int salesArrangementTypeId)
    {
        ValidateRefinancingPermissions(salesArrangementTypeId, UserPermissions.DOCUMENT_SIGNING_RefinancingManage, UserPermissions.DOCUMENT_SIGNING_Manage);
    }

    public void ValidateRefinancingPermissions(in int salesArrangementTypeId, in UserPermissions refinancingPermission, in UserPermissions nonRefinancingPermission)
    {
        // retence
        if (ISalesArrangementAuthorizationService.RefinancingSATypes.Contains(salesArrangementTypeId) && !_currentUser.HasPermission(refinancingPermission))
        {
            throw new CisAuthorizationException($"Missing permission {refinancingPermission} for SalesArrangementTypeId {salesArrangementTypeId}");
        }
        // ostatni typy SA
        else if (ISalesArrangementAuthorizationService.NonRefinancingSATypes.Contains(salesArrangementTypeId) && !_currentUser.HasPermission(nonRefinancingPermission))
        {
            throw new CisAuthorizationException($"Missing permission {nonRefinancingPermission} for SalesArrangementTypeId {salesArrangementTypeId}");
        }
    }

    public void ValidateRefinancing241Permission()
    {
        if (_currentUser.HasPermission(UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage))
            return;

        throw new CisAuthorizationException($"User does not have permission {UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage} ({(int)UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage})");
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICurrentUserAccessor _currentUser;

    public SalesArrangementAuthorizationService(ICurrentUserAccessor currentUser, ISalesArrangementServiceClient salesArrangementService)
    {
        _currentUser = currentUser;
        _salesArrangementService = salesArrangementService;
    }
}
