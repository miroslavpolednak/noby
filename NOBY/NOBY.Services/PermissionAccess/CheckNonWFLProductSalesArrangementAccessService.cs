using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Services.CheckNonWFLProductSalesArrangementAccess;

public interface INonWFLProductSalesArrangementAccessService
{
    Task CheckNonWFLProductSalesArrangementAccess(int salesArrangementId, CancellationToken cancellationToken);
}

[ScopedService, AsImplementedInterfacesService]
public class NonWFLProductSalesArrangementAccess(
    ISalesArrangementServiceClient _salesArrangementService,
    ICaseServiceClient _caseService) 
    : INonWFLProductSalesArrangementAccessService
{
    public async Task CheckNonWFLProductSalesArrangementAccess(int salesArrangementId, CancellationToken cancellationToken)
    {
        var sa = await _salesArrangementService.ValidateSalesArrangementId(salesArrangementId, true, cancellationToken);
        var caseValidationResult = await _caseService.ValidateCaseId(sa.CaseId!.Value, true, cancellationToken);

        // State > 1 (není ve stavu tvorby žádosti)
        if (caseValidationResult.IsInState(CaseHelpers.AllExceptInProgressStates) && sa.SalesArrangementTypeId!.Value == (int)SalesArrangementTypes.Mortgage)
        {
            throw new CisAuthorizationException("Cannot access endpoint, if Case.State > 1 and Sales Arrangement Type is for Mortgage");
        }
    }
}
