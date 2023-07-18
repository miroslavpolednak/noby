using System.ComponentModel;

namespace NOBY.Dto.RealEstateValuation.SpecificDetails;

/// <summary>
/// Objekt je použit pouze v případě, že jde o variantu nemovitosti HF (Dům a byt) nebo HF+F (Byt)
/// </summary>
public sealed class HouseAndFlatDetails
{
    /// <summary>
    /// True pokud jde o zanedbanou nemovitost
    /// </summary>
    /// <example>false</example>
    [Required, DefaultValue(false)]
    public bool PoorCondition { get; init; }

    /// <summary>
    /// True pokud jsou vlastnická práva nějakým způsobem omezena
    /// </summary>
    /// <example>false</example>
    [Required, DefaultValue(false)]
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
        /// <example>false</example>
        [Required, DefaultValue(false)]
        public bool SpecialPlacement { get; init; }

        /// <summary>
        /// True pokud jde o umístění v suterénu
        /// </summary>
        /// <example>false</example>
        [Required, DefaultValue(false)]
        public bool Basement { get; init; }
    }

    public sealed class FinishedHouseAndFlatDetailsDto
    {
        /// <summary>
        /// True pokud je nemovitost pronajata
        /// </summary>
        /// <example>false</example>
        [Required, DefaultValue(false)]
        public bool Leased { get; init; }

        /// <summary>
        /// Obecná pronajímatelnost
        /// </summary>
        /// <example>false</example>
        [Required, DefaultValue(false)]
        public bool LeaseApplicable { get; init; }
    }
}