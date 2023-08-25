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