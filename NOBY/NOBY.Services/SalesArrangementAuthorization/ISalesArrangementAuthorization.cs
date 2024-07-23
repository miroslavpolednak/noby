using DomainServices.UserService.Clients.Authorization;
using System.Collections.ObjectModel;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Services.SalesArrangementAuthorization;

public interface ISalesArrangementAuthorizationService
{
    List<SalesArrangement> FiltrSalesArrangements(IEnumerable<SalesArrangement> salesArrangements);

    Task ValidateSaAccessBySaType213And248BySAId(int salesArrangementId, CancellationToken cancellationToken);

    Task ValidateDocumentSigningMngBySaType237And246BySAId(int salesArrangementId, CancellationToken cancellationToken);

    void ValidateSaAccessBySaType213And248(in int salesArrangementTypeId);

    void ValidateDocumentSigningMngBySaType237And246(in int salesArrangementTypeId);

    void ValidateRefinancingPermissions(in int salesArrangementTypeId, in UserPermissions refinancingPermission, in UserPermissions nonRefinancingPermission);

    public static ReadOnlyCollection<int> RefinancingSATypes => _refinancingSATypes;
    private static ReadOnlyCollection<int> _refinancingSATypes = (new int[]
        {
            (int)SalesArrangementTypes.MortgageRefixation,
            (int)SalesArrangementTypes.MortgageRetention,
            (int)SalesArrangementTypes.MortgageExtraPayment
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

    void ValidateRefinancing241Permission();
}
