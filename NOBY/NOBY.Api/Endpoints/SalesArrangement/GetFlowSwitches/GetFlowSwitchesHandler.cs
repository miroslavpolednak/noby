using CIS.Foms.Enums;

namespace NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;

internal sealed class GetFlowSwitchesHandler
    : IRequestHandler<GetFlowSwitchesRequest, GetFlowSwitchesResponse>
{
    public async Task<GetFlowSwitchesResponse> Handle(GetFlowSwitchesRequest request, CancellationToken cancellationToken)
    {
        // vytahnout flow switches z SA
        var existingSwitches = await _salesArrangementService.GetFlowSwitches(request.SalesArrangementId, cancellationToken);
        
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

        if (response.SigningSection.IsCompleted)
        {
            var documentsToSignListResponse = await _documentOnSaService.GetDocumentsToSignList(request.SalesArrangementId, cancellationToken);
            var allDocumentSigned = documentsToSignListResponse.DocumentsOnSAToSign.All(d => d.IsSigned);

            if (!allDocumentSigned)
            {
                response.SigningSection.IsCompleted = false;
            }
        }
        
        return response;
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

    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;
    private readonly Infrastructure.Services.FlowSwitches.IFlowSwitchesService _flowSwitches;
    private readonly DomainServices.DocumentOnSAService.Clients.IDocumentOnSAServiceClient _documentOnSaService;

    public GetFlowSwitchesHandler(
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient arrangementServiceClient,
        Infrastructure.Services.FlowSwitches.IFlowSwitchesService flowSwitches,
        DomainServices.DocumentOnSAService.Clients.IDocumentOnSAServiceClient documentOnSaService)
    {
        _flowSwitches = flowSwitches;
        _documentOnSaService = documentOnSaService;
        _salesArrangementService = arrangementServiceClient;
    }
}
