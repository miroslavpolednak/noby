using DomainServices.CaseService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Services.PermissionAccess;

public interface INonWFLProductSalesArrangementAccess
{
    Task CheckNonWFLProductSalesArrangementAccess(int salesArrangementId, CancellationToken cancellationToken);
}

[ScopedService, AsImplementedInterfacesService]
public class NonWFLProductSalesArrangementAccess : INonWFLProductSalesArrangementAccess
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;

    public NonWFLProductSalesArrangementAccess(
        ISalesArrangementServiceClient salesArrangementService,
        ICaseServiceClient caseService)
    {
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
    }

    public async Task CheckNonWFLProductSalesArrangementAccess(int salesArrangementId, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
        var caseValidationResult = await _caseService.ValidateCaseId(salesArrangement.CaseId, true, cancellationToken);

        // State > 1 (není ve stavu tvorby žádosti)
        if (caseValidationResult.State > 1 && salesArrangement.SalesArrangementTypeId == (int)SalesArrangementTypes.Mortgage)
        {
            throw new CisAuthorizationException("Cannot access endpoint, if Case.State > 1 and Sales Arrangement Type is for Mortgage");
        }

    }
}
