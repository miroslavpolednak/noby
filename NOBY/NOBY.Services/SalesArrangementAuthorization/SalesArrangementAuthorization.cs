using CIS.Core.Security;
using NOBY.Infrastructure.Security;
using DomainServices.UserService.Clients.Authorization;
using DomainServices.SalesArrangementService.Clients;
using System.Collections.ObjectModel;

namespace NOBY.Services.SalesArrangementAuthorization;

public interface ISalesArrangementAuthorizationService
{
    Task ValidateSaAccessBySaType213And248BySAId(int salesArrangementId, CancellationToken cancellationToken);

    Task ValidateDocumentSigningMngBySaType237And246BySAId(int salesArrangementId, CancellationToken cancellationToken);

    void ValidateSaAccessBySaType213And248(in int salesArrangementTypeId);

    void ValidateDocumentSigningMngBySaType237And246(in int salesArrangementTypeId);

    void ValidateRefinancingPermissions(in int salesArrangementTypeId, in UserPermissions refinancingPermission, in UserPermissions nonRefinancingPermission);

    public static ReadOnlyCollection<int> RefinancingSATypes => _refinancingSATypes;
    private static ReadOnlyCollection<int> _refinancingSATypes = (new int[]
        {
            (int)SalesArrangementTypes.Refixation,
            (int)SalesArrangementTypes.Retention,
            (int)SalesArrangementTypes.MimoradnaSplatka
        }).AsReadOnly();

    public static ReadOnlyCollection<int> NonRefinancingSATypes => _nonRefinancingSATypes;
    private static ReadOnlyCollection<int> _nonRefinancingSATypes = (new int[]
        {
            (int)SalesArrangementTypes.Mortgage,
            (int)SalesArrangementTypes.Drawing,
            (int)SalesArrangementTypes.GeneralChange,
            (int)SalesArrangementTypes.HUBN,
            (int)SalesArrangementTypes.CustomerChange,
            (int)SalesArrangementTypes.CustomerChange3602A,
            (int)SalesArrangementTypes.CustomerChange3602B,
            (int)SalesArrangementTypes.CustomerChange3602C
        }).AsReadOnly();
}

[ScopedService, AsImplementedInterfacesService]
internal sealed class SalesArrangementAuthorizationService
    : ISalesArrangementAuthorizationService
{
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

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICurrentUserAccessor _currentUser;

    public SalesArrangementAuthorizationService(ICurrentUserAccessor currentUser, ISalesArrangementServiceClient salesArrangementService)
    {
        _currentUser = currentUser;
        _salesArrangementService = salesArrangementService;
    }
}
