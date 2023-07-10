using CIS.Core.Security;
using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Dto.RealEstateValuation;
using Helpers = DomainServices.RealEstateValuationService.Contracts.Helpers;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetListRealEstateValuation;

internal sealed class GetListRealEstateValuationHandler
    : IRequestHandler<GetListRealEstateValuationRequest, List<RealEstateValuationListItem>>
{
    public async Task<List<RealEstateValuationListItem>> Handle(GetListRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        // perm check
        if (caseInstance.CaseOwner.UserId != _currentUser.User!.Id && !_currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
        {
            throw new CisAuthorizationException();
        }

        // dopocitana oceneni na zaklade dat v SA
        List<RealEstateValuationListItem>? computedValuations = null;
        if (caseInstance.State == (int)CaseStates.InProgress)
        {
            computedValuations = await getComputedValuations(request.CaseId, cancellationToken);
        }

        // oceneni ulozena u nas v DB
        var existingValuations = await getExistingValuations(request.CaseId, cancellationToken);

        if (computedValuations?.Any() ?? false)
        {
            var arr = existingValuations.Where(t => t.IsLoanRealEstate).Select(t => t.RealEstateTypeId).ToArray();
            existingValuations.AddRange(computedValuations.Where(t => !arr.Contains(t.RealEstateTypeId)));
            return existingValuations;
        }
        else
        {
            return existingValuations;
        }
    }

    /// <summary>
    /// Ziskat dopocitane nemovitosti - tj. ty, ktere jsou zadane na Offer pro dany SA
    /// </summary>
    private async Task<List<RealEstateValuationListItem>?> getComputedValuations(long caseId, CancellationToken cancellationToken)
    {
        var saId = (await _salesArrangementService.GetProductSalesArrangement(caseId, cancellationToken)).SalesArrangementId;
        var saInstance = await _salesArrangementService.GetSalesArrangement(saId, cancellationToken);

        var developer = await _offerService.GetOfferDeveloper(saInstance.OfferId!.Value, cancellationToken);

        var state = (await _codebookService.WorkflowTaskStatesNoby(cancellationToken))
            .First(t => t.Id == 6);

        return saInstance.Mortgage
            .LoanRealEstates?
            .Where(t => t.IsCollateral)
            .Select(t => new RealEstateValuationListItem
            {
                CaseId = saInstance.CaseId,
                RealEstateTypeId = t.RealEstateTypeId,
                RealEstateTypeIcon = Helpers.GetRealEstateTypeIcon(t.RealEstateTypeId),
                ValuationStateId = state.Id,
                ValuationStateIndicator = (ValuationStateIndicators)state.Indicator,
                ValuationStateName = state.Name,
                IsLoanRealEstate = true,
                DeveloperAllowed = developer.IsDeveloperAllowed
            })
            .ToList();
    }

    /// <summary>
    /// Ziskat realne ulozene nemovitosti, ktere mame u nas v DB
    /// </summary>
    private async Task<List<RealEstateValuationListItem>> getExistingValuations(long caseId, CancellationToken cancellationToken)
    {
        var revList = await _realEstateValuationService.GetRealEstateValuationList(caseId, cancellationToken);

        var states = await _codebookService.WorkflowTaskStatesNoby(cancellationToken);

        return revList.Select(item => item.MapToApiResponse(states)).ToList();
    }

    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public GetListRealEstateValuationHandler(
        IOfferServiceClient offerService,
        ISalesArrangementServiceClient salesArrangementService,
        ICodebookServiceClient codebookService,
        IRealEstateValuationServiceClient realEstateValuationService,
        ICaseServiceClient caseService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _codebookService = codebookService;
        _currentUser = currentUserAccessor;
        _realEstateValuationService = realEstateValuationService;
    }
}
