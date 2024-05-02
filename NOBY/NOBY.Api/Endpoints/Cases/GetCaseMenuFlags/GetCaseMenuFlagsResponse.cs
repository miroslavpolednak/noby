namespace NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;

public sealed class GetCaseMenuFlagsResponse
{
    /// <summary>
    /// Položka menu Dokumenty
    /// </summary>
    public GetCaseMenuFlagsItem DocumentsMenuItem { get; set; } = null!;

    /// <summary>
    /// Položka menu Podmínky ke splnění
    /// </summary>
    public GetCaseMenuFlagsItem CovenantsMenuItem { get; set; } = null!;

    /// <summary>
    /// Položka menu Nemovitosti
    /// </summary>
    public GetCaseMenuFlagsItem RealEstatesMenuItem { get; set; } = null!;

    /// <summary>
    /// Položka menu Parametry
    /// </summary>
    public GetCaseMenuFlagsItem ParametersMenuItem { get; set; } = null!;

    /// <summary>
    /// Položka menu Žadatelé
    /// </summary>
    public GetCaseMenuFlagsItem DebtorsItem { get; set; } = null!;

    /// <summary>
    /// Položka menu Požadavky a změny
    /// </summary>
    public GetCaseMenuFlagsItem ChangeRequestsMenuItem { get; set; } = null!;

    /// <summary>
    /// Položka menu Úkoly
    /// </summary>
    public GetCaseMenuFlagsItem TasksMenuItem { get; set; } = null!;

    /// <summary>
    /// Změna úrokové sazby 
    /// </summary>
    public GetCaseMenuFlagsItem RefinancingMenuItem { get; set; } = null!;

    /// <summary>
    /// Mimořádná splátka
    /// </summary>
    public GetCaseMenuFlagsItem ExtraPaymentMenuItem { get; set; } = null!;
}

public sealed class GetCaseMenuFlagsItem
{
    /// <summary>
    /// Příznak
    /// </summary>
    public GetCaseMenuFlagsTypes Flag { get; set; }

    public bool IsActive { get; set; } = true;
}

public enum GetCaseMenuFlagsTypes
{
    NoFlag = 0,
    InProcessing = 1,
    ExclamationMark = 2

}