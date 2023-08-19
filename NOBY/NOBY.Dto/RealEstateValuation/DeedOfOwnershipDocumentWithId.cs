namespace NOBY.Dto.RealEstateValuation;

public sealed class DeedOfOwnershipDocumentWithId
{
    /// <summary>
    /// Noby ID daného záznamu.Určuje jednoznačnou kombinaci cremDeedOfOwnershipDocumentId a RealEstateValuationId (Noby Ocenění) pro případy simulování více možností žádostí s jednou nemovitostí.
    /// </summary>
    public int DeedOfOwnershipDocumentId { get; set; }

    /// <summary>
    /// Identifikační údaje nemovitosti k Ocenění(bez Noby ID)
    /// </summary>
    public DeedOfOwnershipDocument? DeedOfOwnershipDocument { get; set; }
}