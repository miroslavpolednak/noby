namespace NOBY.Api.Endpoints.Cases.GetCaseDocumentsFlag;

public sealed class GetCaseMenuFlagsResponse
{
    public GetCaseMenuFlagsItem DocumentsMenuItem { get; set; } = null!;

    public GetCaseMenuFlagsItem CovenantsMenuItem { get; set; } = null!;
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
    ExclamationMark = 1,
    InProcessing = 2

}