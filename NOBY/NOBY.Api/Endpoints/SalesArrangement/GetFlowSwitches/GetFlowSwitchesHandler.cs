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

        return new GetFlowSwitchesResponse
        {
            ModelationSection = createSection(mergedSwitches[CIS.Foms.Enums.FlowSwitchesGroups.ModelationSection]),
            //IndividualPriceSection = createSection(mergedSwitches[CIS.Foms.Enums.FlowSwitchesGroups.IndividualPriceSection]),
            HouseholdSection = createSection(mergedSwitches[CIS.Foms.Enums.FlowSwitchesGroups.HouseholdSection]),
            ParametersSection = createSection(mergedSwitches[CIS.Foms.Enums.FlowSwitchesGroups.ParametersSection]),
            SigningSection = createSection(mergedSwitches[CIS.Foms.Enums.FlowSwitchesGroups.SigningSection]),
            ScoringSection = createSection(mergedSwitches[CIS.Foms.Enums.FlowSwitchesGroups.ScoringSection]),
            EvaluationSection = createSection(mergedSwitches[CIS.Foms.Enums.FlowSwitchesGroups.EvaluationSection]),
            SendButton = createSection(mergedSwitches[CIS.Foms.Enums.FlowSwitchesGroups.SendButton])
        };
    }

    private static GetFlowSwitchesResponseItem createSection(Infrastructure.Services.FlowSwitches.FlowSwitchGroup group)
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

    public GetFlowSwitchesHandler(
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient arrangementServiceClient,
        Infrastructure.Services.FlowSwitches.IFlowSwitchesService flowSwitches)
    {
        _flowSwitches = flowSwitches;
        _salesArrangementService = arrangementServiceClient;
    }
}
