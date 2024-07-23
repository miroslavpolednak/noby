using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Services.CheckNonWFLProductSalesArrangementAccess;

public interface INonWFLProductSalesArrangementAccessService
{
    Task CheckNonWFLProductSalesArrangementAccess(int salesArrangementId, CancellationToken cancellationToken);
}

[ScopedService, AsImplementedInterfacesService]
public class NonWFLProductSalesArrangementAccess : INonWFLProductSalesArrangementAccessService
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
        var sa = await _salesArrangementService.ValidateSalesArrangementId(salesArrangementId, true, cancellationToken);
        var caseValidationResult = await _caseService.ValidateCaseId(sa.CaseId!.Value, true, cancellationToken);

        // State > 1 (není ve stavu tvorby žádosti)
        if (caseValidationResult.State > (int)CaseStates.InProgress && sa.SalesArrangementTypeId!.Value == (int)SalesArrangementTypes.Mortgage)
        {
            throw new CisAuthorizationException("Cannot access endpoint, if Case.State > 1 and Sales Arrangement Type is for Mortgage");
        }
    }
}
