namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipIds;

/// <summary>
/// Id položek z listu vlastnicví
/// </summary>
public sealed class GetDeedOfOwnershipIdsResponse
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public List<GetDeedOfOwnershipIdsResponseFlat> Flats { get; set; }

    /// <summary>
    /// ISKN ID listu vlastnictví(LV), technický identifikátor katastru
    /// </summary>
    public long DeedOfOwnershipId { get; set; }
}

/// <summary>
/// Byt
/// </summary>
public sealed class GetDeedOfOwnershipIdsResponseFlat
{
    /// <summary>
    /// Číslo bytu včetně čísla domu
    /// </summary>
    public int FlatNumber { get; set; }

    /// <summary>
    /// ISKN ID listu vlastnictví (LV), technický identifikátor katastru
    /// </summary>
    public long DeedOfOwnershipId { get; set; }

    /// <summary>
    /// Způsob využití bytu
    /// </summary>
    public string MannerOfUseFlatShortName { get; set; } = string.Empty;
}