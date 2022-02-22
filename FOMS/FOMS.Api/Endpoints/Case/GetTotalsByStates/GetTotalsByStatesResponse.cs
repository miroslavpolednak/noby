namespace FOMS.Api.Endpoints.Case.GetTotalsByStates;

public sealed class GetTotalsByStatesResponse
{
    /// <summary>
    /// Stav Case-u. Ciselnik 'CaseStates'.
    /// </summary>
    public int State { get; set; }
    
    /// <summary>
    /// Pocet Cases v danem stavu.
    /// </summary>
    public int Count { get; set; }
}
