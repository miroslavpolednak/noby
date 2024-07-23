using CIS.Core.Security;

namespace NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;

internal sealed class GetFlowSwitchesHandler(
    ICurrentUserAccessor _currentUserAccessor,
    DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService,
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
            response.ModelationSection.State = response.ModelationSection.State == 1 && !existingSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsWflTaskForIPNotApproved && t.Value) ? 1 : 2;
        }

        await adjustSigning(response, request.SalesArrangementId, cancellationToken);
        
        await adjustEvaluation(response, request.SalesArrangementId, existingSwitches, cancellationToken);

        adjustIndividualPrice(response, existingSwitches);

        adjustSendButton(response, existingSwitches);

        adjustScoring(response, existingSwitches);

        await setIndicators(response, cancellationToken);

        return response;
    }

    private async Task setIndicators(SalesArrangementGetFlowSwitchesResponse response, CancellationToken cancellationToken)
    {
        var cb = await _codebookService.FlowSwitchStates(cancellationToken);

        response.ModelationSection.StateIndicator = ind(response.ModelationSection.State);
        response.IndividualPriceSection.StateIndicator = ind(response.IndividualPriceSection.State);
        response.HouseholdSection.StateIndicator = ind(response.HouseholdSection.State);
        response.ParametersSection.StateIndicator = ind(response.ParametersSection.State);
        response.SigningSection.StateIndicator = ind(response.SigningSection.State);
        response.ScoringSection.StateIndicator = ind(response.ScoringSection.State);
        response.EvaluationSection.StateIndicator = ind(response.EvaluationSection.State);

        response.ModelationSection.StateName = name(response.ModelationSection.State);
        response.IndividualPriceSection.StateName = name(response.IndividualPriceSection.State);
        response.HouseholdSection.StateName = name(response.HouseholdSection.State);
        response.ParametersSection.StateName = name(response.ParametersSection.State);
        response.SigningSection.StateName = name(response.SigningSection.State);
        response.ScoringSection.StateName = name(response.ScoringSection.State);
        response.EvaluationSection.StateName = name(response.EvaluationSection.State);

        string name(int id)
            => cb.First(t => t.Id == id).Name;

        EnumStateIndicators ind(int id)
            => (EnumStateIndicators)cb.First(t => t.Id == id).Indicator;
    }

    private static void adjustIndividualPrice(SalesArrangementGetFlowSwitchesResponse response, List<DomainServices.SalesArrangementService.Contracts.FlowSwitch> existingSwitches)
    {
        if (isSet(FlowSwitches.DoesWflTaskForIPExist) && isSet(FlowSwitches.IsWflTaskForIPApproved))
        {
            response.IndividualPriceSection.State = 3;
        }
        else if (isSet(FlowSwitches.DoesWflTaskForIPExist) && isSet(FlowSwitches.IsWflTaskForIPNotApproved))
        {
            response.IndividualPriceSection.State = 4;
        }
        else if (isSet(FlowSwitches.DoesWflTaskForIPExist) && !isSet(FlowSwitches.IsWflTaskForIPApproved) && !isSet(FlowSwitches.IsWflTaskForIPNotApproved))
        {
            response.IndividualPriceSection.State = 5;
        }

        bool isSet(FlowSwitches fs)
            => existingSwitches.Any(t => t.FlowSwitchId == (int)fs && t.Value);
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
            response.SendButton.IsActive = response.ModelationSection.State == 1
                && response.HouseholdSection.State == 1
                && response.ParametersSection.State == 1
                && response.SigningSection.State == 1
                && response.ScoringSection.State == 1
                // valuation
                && (response.EvaluationSection.State == 1 || flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsRealEstateValuationAllowed && !t.Value))
                // IC
                && (response.IndividualPriceSection.State == 1 || icSection());
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
            if (valuations.Count != 0 || (response.ScoringSection.State == 1 && documentsToSignListResponse.All(t => t.IsSigned)))
            {
                response.EvaluationSection.IsActive = true;

                var saInstanceDetail = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
                var A = valuations.Count(t => t.IsLoanRealEstate && (t.OrderId.HasValue || (t.DeveloperAllowed && t.DeveloperApplied)));
                var B = saInstanceDetail.Mortgage?.LoanRealEstates?.Count(t => t.IsCollateral) ?? 0;
                var C = valuations.Count(t => t.OrderId.HasValue);
                response.EvaluationSection.State = (B > 0 && A == B) || (C > 0 && B == 0) ? 1 : 2;
            }
        }
    }

    private async Task adjustSigning(SalesArrangementGetFlowSwitchesResponse response, int salesArrangementId, CancellationToken cancellationToken)
    {
        if (response.SigningSection.State == 1)
        {
            var documentsToSignListResponse = await getDocumentsToSign(salesArrangementId, cancellationToken);

            if (documentsToSignListResponse.All(t => t.IsSigned))
            {
                if (documentsToSignListResponse.Any(t =>
                    t.SignatureTypeId == (int)SignatureTypes.Paper
                    && !(t.EArchivIdsLinked?.Any() ?? false)))
                {
                    response.SigningSection.State = 2;
                }
            }
            else
            {
                response.ScoringSection.IsActive = false;
                response.SigningSection.State = 2;
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
            State = group.State,
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
