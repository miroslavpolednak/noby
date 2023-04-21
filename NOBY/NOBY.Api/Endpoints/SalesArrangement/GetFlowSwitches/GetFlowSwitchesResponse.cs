namespace NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;

public sealed class GetFlowSwitchesResponse
{
    public GetFlowSwitchesResponseItem ModelationSection { get; set; } = null!;
    public GetFlowSwitchesResponseItem HouseholdSection { get; set; } = null!;
    public GetFlowSwitchesResponseItem ParametersSection { get; set; } = null!;
    public GetFlowSwitchesResponseItem ScoringSection { get; set; } = null!;
    public GetFlowSwitchesResponseItem SigningSection { get; set; } = null!;
    public GetFlowSwitchesResponseItem EvaluationSection { get; set; } = null!;
    public GetFlowSwitchesResponseItemButton SendButton { get; set; } = null!;
}

public sealed class GetFlowSwitchesResponseItemButton
{
    /// <summary>
    /// Je sekce aktivní/neaktivní (lze prokliknout)?
    /// </summary>
    public bool IsActive { get; set; }
}

public sealed class GetFlowSwitchesResponseItem
{
    /// <summary>
    /// Je sekce skrytá?
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// Je sekce aktivní/neaktivní (lze prokliknout)?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Je v sekci vše splněné?
    /// </summary>
    public bool IsCompleted { get; set; }
}