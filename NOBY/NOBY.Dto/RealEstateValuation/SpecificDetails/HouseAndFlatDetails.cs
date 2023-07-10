namespace NOBY.Dto.RealEstateValuation.SpecificDetails;

/// <summary>
/// Objekt je použit pouze v případě, že jde o variantu nemovitosti HF (Dům a byt) nebo HF+F (Byt)
/// </summary>
public sealed class HouseAndFlatDetails
{
    /// <summary>
    /// True pokud jde o zanedbanou nemovitost
    /// </summary>
    [Required]
    public bool PoorCondition { get; init; }

    /// <summary>
    /// True pokud jsou vlastnická práva nějakým způsobem omezena
    /// </summary>
    [Required]
    public bool OwnershipRestricted { get; init; }

    /// <summary>
    /// Objekt je použit pouze v případě varianty nemovitosti HF+F Byt.
    /// </summary>
    public FlatOnlyDetailsDto? FlatOnlyDetails { get; init; }

    /// <summary>
    /// Objekt je použit pouze v případě, že jde o variantu nemovitosti HF (Dům a byt) nebo HF+F (Byt) a že je nemovitost ve stavu "Dokončená".
    /// </summary>
    public FinishedHouseAndFlatDetailsDto? FinishedHouseAndFlatDetails { get; init; }

    public sealed class FlatOnlyDetailsDto
    {
        /// <summary>
        /// True pokud je byt v rodinném, jiném než bytovém domě
        /// </summary>
        [Required]
        public bool SpecialPlacement { get; init; }

        /// <summary>
        /// True pokud jde o umístění v suterénu
        /// </summary>
        [Required]
        public bool Basement { get; init; }
    }

    public sealed class FinishedHouseAndFlatDetailsDto
    {
        /// <summary>
        /// True pokud je nemovitost pronajata
        /// </summary>
        [Required]
        public bool Leased { get; init; }

        /// <summary>
        /// Obecná pronajímatelnost
        /// </summary>
        [Required]
        public bool LeaseApplicable { get; init; }
    }
}