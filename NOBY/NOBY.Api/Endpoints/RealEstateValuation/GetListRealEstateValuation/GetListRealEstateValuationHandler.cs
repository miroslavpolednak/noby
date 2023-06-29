using CIS.Core.Security;
using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using DomainServices.RealEstateValuationService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts.v1;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetListRealEstateValuation;

internal sealed class GetListRealEstateValuationHandler
    : IRequestHandler<GetListRealEstateValuationRequest>
{
    public async Task Handle(GetListRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        // perm check
        if (caseInstance.CaseOwner.UserId != _currentUser.User!.Id && !_currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
        {
            throw new CisAuthorizationException();
        }

        List<RealEstateValuationListItem> computedValuations = new();
        if (caseInstance.State != (int)CaseStates.InProgress)
        {
            var saId = (await _salesArrangementService.GetProductSalesArrangement(request.CaseId, cancellationToken)).SalesArrangementId;

            var saInstance = await _salesArrangementService.GetSalesArrangement(saId, cancellationToken);

        }

        var existingValuations = await getExistingValuations(request.CaseId, cancellationToken);

    }

    private async Task<List<RealEstateValuationListItem>> getExistingValuations(long caseId, CancellationToken cancellationToken)
    {
        var revList = await _realEstateValuationService.GetRealEstateValuationList(caseId, cancellationToken);

        var states = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);

        return revList
            .Select(t =>
            {
                // tvrdi ze tam bude vzdy zaznam a v EA neni zadne osetreni...
                var state = states.First(x => x.Id == t.ValuationStateId);

                return new RealEstateValuationListItem
                {
                    RealEstateValuationId = t.RealEstateValuationId,
                    OrderId = t.OrderId,
                    CaseId = t.CaseId,
                    RealEstateTypeId = t.RealEstateTypeId,
                    RealEstateTypeIcon = Helpers.GetRealEstateTypeIcon(t),
                    ValuationStateId = t.RealEstateValuationId,
                    ValuationStateIndicator = state.Indicator,
                    ValuationStateName = state.Name,
                    IsLoanRealEstate = t.IsLoanRealEstate,
                    RealEstateStateId = t.RealEstateStateId.GetValueOrDefault(),
                    ValuationTypeId = t.ValuationTypeId,
                    Address = t.Address,
                    ValuationSentDate = t.ValuationSentDate,
                    ValuationResultCurrentPrice = t.ValuationResultCurrentPrice,
                    ValuationResultFuturePrice = t.ValuationResultFuturePrice,
                    IsRevaluationRequired = t.IsRevaluationRequired,
                    DeveloperAllowed = t.DeveloperAllowed,
                    DeveloperApplied = t.DeveloperApplied
                };
            })
            .ToList();
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public GetListRealEstateValuationHandler(
        ISalesArrangementServiceClient salesArrangementService,
        ICodebookServiceClient codebookService,
        IRealEstateValuationServiceClient realEstateValuationService,
        ICaseServiceClient caseService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _codebookService = codebookService;
        _currentUser = currentUserAccessor;
        _realEstateValuationService = realEstateValuationService;
    }
}
