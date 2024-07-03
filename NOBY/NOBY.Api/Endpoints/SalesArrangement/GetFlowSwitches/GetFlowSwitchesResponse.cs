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
    public GetFlowSwitchesResponseItem IndividualPriceSection { get; set; } = null!;
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
    /// Název stavu
    /// </summary>
    public string StateName { get; set; }

    /// <summary>
    /// Hodnota z číselníku FlowSwitchState
    /// </summary>
    public int State { get; set; }

    /// <summary>
    /// Indikátor stavu knedlíku, 0 - Unknown, 1 - Active, 2 - Cancelled, 3 - OK, 4 - Passive, 5 - Warning, 6 - Initial
    /// </summary>
    public StateIndicators StateIndicator { get; set; }
}