using CIS.Foms.Enums;

namespace NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;

internal sealed class GetFlowSwitchesHandler
    : IRequestHandler<GetFlowSwitchesRequest, GetFlowSwitchesResponse>
{
    public async Task<GetFlowSwitchesResponse> Handle(GetFlowSwitchesRequest request, CancellationToken cancellationToken)
    {
        // vytahnout flow switches z SA
        var existingSwitches = await _flowSwitches.GetFlowSwitchesForSA(request.SalesArrangementId, cancellationToken);
        
        // zjistit stav jednotlivych sekci na FE
        var mergedSwitches = _flowSwitches.GetFlowSwitchesGroups(existingSwitches);

        var response = new GetFlowSwitchesResponse
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

        return response;
    }

    private static void adjustSendButton(GetFlowSwitchesResponse response, List<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitches)
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

        bool icSection()
        {
            return flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsOfferWithDiscount && t.Value)
                && flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.DoesWflTaskForIPExist && t.Value)
                && flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsWflTaskForIPNotApproved && !t.Value);
        }
    }

    private async Task adjustEvaluation(
        GetFlowSwitchesResponse response, 
        int salesArrangementId, 
        List<DomainServices.SalesArrangementService.Contracts.FlowSwitch> flowSwitches, 
        CancellationToken cancellationToken) 
    {
        var saInstance = await _salesArrangementService.ValidateSalesArrangementId(salesArrangementId, false, cancellationToken);
        var valuations = await _realEstateValuationService.GetRealEstateValuationList(saInstance.CaseId!.Value, cancellationToken);

        if (valuations.Any()
            || flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsRealEstateValuationAllowed && t.Value))
        {
            response.EvaluationSection.IsVisible = true;

            var documentsToSignListResponse = await getDocumentsToSign(salesArrangementId, cancellationToken);
            if (valuations.Any() || (response.ScoringSection.IsCompleted && documentsToSignListResponse.All(t => t.IsSigned)))
            {
                response.EvaluationSection.IsActive = true;

                var saInstanceDetail = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
                var A = valuations.Count(t => t.IsLoanRealEstate && t.OrderId.HasValue);
                var B = saInstanceDetail.Mortgage?.LoanRealEstates?.Count ?? 0;
                response.EvaluationSection.IsCompleted = (B > 0 && A == B) || (A == 1 && B == 0);
            }
        }
    }

    private async Task adjustSigning(GetFlowSwitchesResponse response, int salesArrangementId, CancellationToken cancellationToken)
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
            }
        }
    }

    private static GetFlowSwitchesResponseItemButton createSectionButton(NOBY.Dto.FlowSwitches.FlowSwitchGroup group)
    {
        return new GetFlowSwitchesResponseItemButton
        {
            IsActive = group.IsActive
        };
    }

    private static GetFlowSwitchesResponseItem createSection(NOBY.Dto.FlowSwitches.FlowSwitchGroup group)
    {
        return new GetFlowSwitchesResponseItem
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

    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly Services.FlowSwitches.IFlowSwitchesService _flowSwitches;
    private readonly DomainServices.DocumentOnSAService.Clients.IDocumentOnSAServiceClient _documentOnSaService;
    private readonly DomainServices.RealEstateValuationService.Clients.IRealEstateValuationServiceClient _realEstateValuationService;

    public GetFlowSwitchesHandler(
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient arrangementServiceClient,
        DomainServices.RealEstateValuationService.Clients.IRealEstateValuationServiceClient realEstateValuationService,
        Services.FlowSwitches.IFlowSwitchesService flowSwitches,
        DomainServices.DocumentOnSAService.Clients.IDocumentOnSAServiceClient documentOnSaService)
    {
        _realEstateValuationService = realEstateValuationService;
        _flowSwitches = flowSwitches;
        _documentOnSaService = documentOnSaService;
        _salesArrangementService = arrangementServiceClient;
    }
}
