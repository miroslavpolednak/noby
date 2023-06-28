using CIS.Core.Security;
using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.CreateRealEstateValuation;

internal sealed class CreateRealEstateValuationHandler
    : IRequestHandler<CreateRealEstateValuationRequest, int>
{
    public async Task<int> Handle(CreateRealEstateValuationRequest request, CancellationToken cancellationToken)
    {
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        // perm check
        if (caseInstance.CaseOwner.UserId != _currentUser.User!.Id && !_currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
        {
            throw new CisAuthorizationException();
        }

        var revRequest = new DomainServices.RealEstateValuationService.Contracts.CreateRealEstateValuationRequest
        {
            CaseId = request.CaseId,
            RealEstateTypeId = request.RealEstateTypeId,
            IsLoanRealEstate = request.IsLoanRealEstate,
            ValuationTypeId = DomainServices.RealEstateValuationService.Contracts.ValuationTypes.Unknown,
            DeveloperApplied = request.DeveloperApplied,
            ValuationStateId = 7
        };

        if (caseInstance.State != (int)CaseStates.InProgress)
        {
            var saInstance = await _salesArrangementService.GetProductSalesArrangement(request.CaseId, cancellationToken);

            var offerInstance = await _offerService.GetMortgageOfferDetail(saInstance.OfferId!.Value, cancellationToken);

            if (offerInstance.SimulationInputs.Developer?.DeveloperId != null 
                && offerInstance.SimulationInputs.Developer?.ProjectId != null
                && revRequest.IsLoanRealEstate)
            {
                var project = await _codebookService.GetDeveloperProject(offerInstance.SimulationInputs.Developer.DeveloperId.Value, offerInstance.SimulationInputs.Developer.ProjectId.Value, cancellationToken);

                revRequest.DeveloperAllowed = _allowedMassValuations.Contains(project.MassEvaluation);
                if (request.DeveloperApplied)
                {
                    revRequest.ValuationStateId = 4;
                }
            }
        }

        if (!revRequest.DeveloperAllowed && request.DeveloperApplied)
        {
            throw new CisAuthorizationException();
        }

        return await _realEstateValuationService.CreateRealEstateValuation(revRequest, cancellationToken);
    }

    private static int[] _allowedMassValuations = new int[] { -1, 1 };

    private readonly ICodebookServiceClient _codebookService;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public CreateRealEstateValuationHandler(
        ICodebookServiceClient codebookService,
        IOfferServiceClient offerService,
        ISalesArrangementServiceClient salesArrangementService,
        ICurrentUserAccessor currentUserAccessor,
        IRealEstateValuationServiceClient realEstateValuationService, 
        ICaseServiceClient caseService)
    {
        _codebookService = codebookService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _currentUser = currentUserAccessor;
        _realEstateValuationService = realEstateValuationService;
        _caseService = caseService;
    }
}
