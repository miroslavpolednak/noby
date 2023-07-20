namespace ExternalServices.Crem.Dto;

/// <summary>
/// Id položek z listu vlastnicví
/// </summary>
public sealed class FlatsForAddress
{
    public List<Flat>? Flats { get; set; }

    /// <summary>
    /// ISKN ID listu vlastnictví(LV), technický identifikátor katastru
    /// </summary>
    public int DeedOfOwnershipId { get; set; }

    /// <summary>
    /// Byt
    /// </summary>
    public sealed class Flat
    {
        /// <summary>
        /// Číslo bytu včetně čísla domu
        /// </summary>
        public int FlatNumber { get; set; }

        /// <summary>
        /// ISKN ID listu vlastnictví (LV), technický identifikátor katastru
        /// </summary>
        public int DeedOfOwnershipId { get; set; }

        /// <summary>
        /// Způsob využití bytu
        /// </summary>
        public string MannerOfUseFlatShortName { get; set; } = string.Empty;
    }
}