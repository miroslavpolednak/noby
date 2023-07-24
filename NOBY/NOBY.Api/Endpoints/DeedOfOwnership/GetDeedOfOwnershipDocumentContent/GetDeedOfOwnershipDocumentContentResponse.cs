namespace NOBY.Api.Endpoints.DeedOfOwnership.GetDeedOfOwnershipDocumentContent;

/// <summary>
/// Informace z listu vlastnictví
/// </summary>
public sealed class GetDeedOfOwnershipDocumentContentResponse
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public List<GetDeedOfOwnershipDocumentContentResponseOwners>? Owners { get; set; }

    public List<GetDeedOfOwnershipDocumentContentResponseLegalRelations>? LegalRelations { get; set; }

    public List<GetDeedOfOwnershipDocumentContentResponseRealEstates>? RealEstates { get; set; }
}

/// <summary>
/// Popis vlastníka na listu vlastnictví
/// </summary>
public sealed class GetDeedOfOwnershipDocumentContentResponseOwners
{
    /// <summary>
    /// Formátovaný popis vlastníka (jméno + adresa)
    /// </summary>
    public string OwnerDescription { get; set; } = string.Empty;

    /// <summary>
    /// Podíl vlastnictví nemovitosti
    /// </summary>
    public string OwnershipRatio { get; set; } = string.Empty;
}

/// <summary>
/// Omezení vlastnických práv na listu vlastnictví
/// </summary>
public sealed class GetDeedOfOwnershipDocumentContentResponseLegalRelations
{
    /// <summary>
    /// Textový popis omezení vlastnického práva
    /// </summary>
    public string LegalRelationDescription { get; set; } = string.Empty;
}

/// <summary>
/// Nemovitost z listu vlastnictví
/// </summary>
public sealed class GetDeedOfOwnershipDocumentContentResponseRealEstates
{
    public long RealEstateId { get; set; }

    /// <summary>
    /// Textový popis nemovitosti
    /// </summary>
    public string RealEstateDescription { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}