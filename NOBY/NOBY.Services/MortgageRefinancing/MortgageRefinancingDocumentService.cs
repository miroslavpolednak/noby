using CIS.Core.Security;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.UserService.Clients.v1;
using DomainServices.UserService.Contracts;
using NOBY.Infrastructure.ErrorHandling;

namespace NOBY.Services.MortgageRefinancing;

[ScopedService, SelfService]
public class MortgageRefinancingDocumentService(
	ISalesArrangementServiceClient _salesArrangementService,
	IUserServiceClient _userService,
	ICurrentUserAccessor _currentUserAccessor,
	MortgageRefinancingWorkflowService _refinancingWorkflowService)
{
	public async Task<SalesArrangement> LoadAndValidateSA(int salesArrangementId, SalesArrangementTypes expectedType, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);

        if (salesArrangement.SalesArrangementTypeId != (int)expectedType)
            throw new NobyValidationException(90032, $"SA is not of type {expectedType}");

        if (GetManagedByRC2(salesArrangement) == true)
            throw new NobyValidationException(90032, "ManagedByRC2 is true or SA is not retention SA");

        if (!salesArrangement.IsInState([EnumSalesArrangementStates.InProgress, EnumSalesArrangementStates.NewArrangement]))
        {
            throw new NobyValidationException(90032, "SA has to be in state InProgress(1) or NewArrangement(5)");
        }

        return salesArrangement;
    }

    public async Task<bool> IsIndividualPriceValid(SalesArrangement salesArrangement, MortgageRefinancingIndividualPrice offerIndividualPrice, CancellationToken cancellationToken)
    {
        var workflowIndividualPrice = await _refinancingWorkflowService.GetIndividualPrices(salesArrangement.CaseId, salesArrangement.ProcessId!.Value, cancellationToken);

        return offerIndividualPrice.Equals(workflowIndividualPrice);
    }

    public async Task<UserInfoObject> LoadUserInfo(CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(_currentUserAccessor.User!.Id, cancellationToken);

        return user.UserInfo;
    }

    private static bool? GetManagedByRC2(SalesArrangement salesArrangement)
    {
        return (SalesArrangementTypes)salesArrangement.SalesArrangementTypeId switch
        {
            SalesArrangementTypes.MortgageRefixation => salesArrangement.Refixation!.ManagedByRC2,
            SalesArrangementTypes.MortgageRetention => salesArrangement.Retention!.ManagedByRC2,
            _ => null
        };
    }
}