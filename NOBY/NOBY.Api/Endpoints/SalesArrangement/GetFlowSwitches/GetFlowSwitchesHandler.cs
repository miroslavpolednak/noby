using CIS.Core.Security;

namespace NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;

internal sealed class GetFlowSwitchesHandler(
    ICurrentUserAccessor _currentUserAccessor,
    DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService,
    DomainServices.RealEstateValuationService.Clients.IRealEstateValuationServiceClient _realEstateValuationService,
    Services.FlowSwitches.IFlowSwitchesService _flowSwitches,
    DomainServices.DocumentOnSAService.Clients.IDocumentOnSAServiceClient _documentOnSaService)
        : IRequestHandler<GetFlowSwitchesRequest, SalesArrangementGetFlowSwitchesResponse>
{
    public async Task<SalesArrangementGetFlowSwitchesResponse> Handle(GetFlowSwitchesRequest request, CancellationToken cancellationToken)
    {
        // vytahnout flow switches z SA
        var existingSwitches = await _flowSwitches.GetFlowSwitchesForSA(request.SalesArrangementId, cancellationToken);
        
        // zjistit stav jednotlivych sekci na FE
        var mergedSwitches = _flowSwitches.GetFlowSwitchesGroups(existingSwitches);

        var response = new SalesArrangementGetFlowSwitchesResponse
        {
            ModelationSection = createSection(mergedSwitches[FlowSwitchesGroups.ModelationSection]),
            IndividualPriceSection = createSection(mergedSwitches[FlowSwitchesGroups.IndividualPriceSection]),
            HouseholdSection = createSection(mergedSwitches[FlowSwitchesGroups.HouseholdSection]),
            ParametersSection = createSection(mergedSwitches[FlowSwitchesGroups.ParametersSection]),
            SigningSection = createSection(mergedSwitches[FlowSwitchesGroups.SigningSection]),
            ScoringSection = createSection(mergedSwitches[FlowSwitchesGroups.ScoringSection]),
            EvaluationSection = createSection(mergedSwitches[FlowSwitchesGroups.EvaluationSection]),
            SendButton = createSectionButton(mergedSwitches[FlowSwitchesGroups.SendButton])
        };

        if (existingSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsOfferWithDiscount && t.Value))
        {
            response.ModelationSection.IsCompleted = response.ModelationSection.IsCompleted && !existingSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsWflTaskForIPNotApproved && t.Value);
        }

        await adjustSigning(response, request.SalesArrangementId, cancellationToken);
        
        await adjustEvaluation(response, request.SalesArrangementId, existingSwitches, cancellationToken);

        adjustSendButton(response, existingSwitches);

        adjustScoring(response, existingSwitches);

        return response;
    }

    private void adjustScoring(SalesArrangementGetFlowSwitchesResponse response, List<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitches)
    {
        if (!flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.ScoringPerformedAtleastOnce && t.Value)
            && !_currentUserAccessor.HasPermission(UserPermissions.SCORING_Perform))
        {
            response.ScoringSection.IsActive = false;
        }
    }

    private void adjustSendButton(SalesArrangementGetFlowSwitchesResponse response, List<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitches)
    {
        if (_currentUserAccessor.HasPermission(UserPermissions.SALES_ARRANGEMENT_Send))
        {
            response.SendButton.IsActive = response.ModelationSection.IsCompleted
                && response.HouseholdSection.IsCompleted
                && response.ParametersSection.IsCompleted
                && response.SigningSection.IsCompleted
                && response.ScoringSection.IsCompleted
                // valuation
                && (response.EvaluationSection.IsCompleted || flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsRealEstateValuationAllowed && !t.Value))
                // IC
                && (response.IndividualPriceSection.IsCompleted || icSection());
        }
        else
        {
            response.SendButton.IsActive = false;
        }

        bool icSection()
        {
            return flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsOfferWithDiscount && !t.Value)
                || (
                    flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsOfferWithDiscount && t.Value)
                    && flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.DoesWflTaskForIPExist && t.Value)
                    && flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsWflTaskForIPNotApproved && !t.Value)
                );
        }
    }

    private async Task adjustEvaluation(
        SalesArrangementGetFlowSwitchesResponse response, 
        int salesArrangementId, 
        List<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitches, 
        CancellationToken cancellationToken) 
    {
        var saInstance = await _salesArrangementService.ValidateSalesArrangementId(salesArrangementId, false, cancellationToken);
        var valuations = await _realEstateValuationService.GetRealEstateValuationList(saInstance.CaseId!.Value, cancellationToken);

        if (valuations.Count != 0
            || flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsRealEstateValuationAllowed && t.Value))
        {
            response.EvaluationSection.IsVisible = true;

            var documentsToSignListResponse = await getDocumentsToSign(salesArrangementId, cancellationToken);
            if (valuations.Count != 0 || (response.ScoringSection.IsCompleted && documentsToSignListResponse.All(t => t.IsSigned)))
            {
                response.EvaluationSection.IsActive = true;

                var saInstanceDetail = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
                var A = valuations.Count(t => t.IsLoanRealEstate && (t.OrderId.HasValue || (t.DeveloperAllowed && t.DeveloperApplied)));
                var B = saInstanceDetail.Mortgage?.LoanRealEstates?.Count(t => t.IsCollateral) ?? 0;
                var C = valuations.Count(t => t.OrderId.HasValue);
                response.EvaluationSection.IsCompleted = (B > 0 && A == B) || (C > 0 && B == 0);
            }
        }
    }

    private async Task adjustSigning(SalesArrangementGetFlowSwitchesResponse response, int salesArrangementId, CancellationToken cancellationToken)
    {
        if (response.SigningSection.IsCompleted)
        {
            var documentsToSignListResponse = await getDocumentsToSign(salesArrangementId, cancellationToken);

            if (documentsToSignListResponse.All(t => t.IsSigned))
            {
                if (documentsToSignListResponse.Any(t =>
                    t.SignatureTypeId == (int)SignatureTypes.Paper
                    && !(t.EArchivIdsLinked?.Any() ?? false)))
                {
                    response.SigningSection.IsCompleted = false;
                }
            }
            else
            {
                response.ScoringSection.IsActive = false;
                response.SigningSection.IsCompleted = false;
            }
        }
    }

    private static SalesArrangementGetFlowSwitchesResponseItemButton createSectionButton(NOBY.Dto.FlowSwitches.FlowSwitchGroup group)
    {
        return new()
        {
            IsActive = group.IsActive
        };
    }

    private static SalesArrangementGetFlowSwitchesResponseItem createSection(NOBY.Dto.FlowSwitches.FlowSwitchGroup group)
    {
        return new()
        {
            IsActive = group.IsActive,
            IsCompleted = group.IsCompleted,
            IsVisible = group.IsVisible
        };
    }

    private async Task<List<DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign>> getDocumentsToSign(int salesArrangementId, CancellationToken cancellationToken)
    {
        if (_documentsOnSAToSign is null)
        {
            _documentsOnSAToSign = (await _documentOnSaService.GetDocumentsToSignList(salesArrangementId, cancellationToken)).DocumentsOnSAToSign.ToList();
        }
        return _documentsOnSAToSign;
    }

    private List<DomainServices.DocumentOnSAService.Contracts.DocumentOnSAToSign>? _documentsOnSAToSign;
}
