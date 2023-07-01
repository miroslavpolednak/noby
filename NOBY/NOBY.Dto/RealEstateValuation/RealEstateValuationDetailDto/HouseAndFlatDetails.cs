﻿using Newtonsoft.Json;

namespace NOBY.Dto.RealEstateValuation.RealEstateValuationDetailDto;

/// <summary>
/// Objekt je použit pouze v případě, že jde o variantu nemovitosti HF (Dům a byt) nebo HF+F (Byt)
/// </summary>
public class HouseAndFlatDetails : ISpecificDetails
{
    /// <summary>
    /// True pokud jde o zanedbanou nemovitost
    /// </summary>
    public bool PoorCondition { get; init; }

    /// <summary>
    /// True pokud jsou vlastnická práva nějakým způsobem omezena
    /// </summary>
    public bool OwnershipRestricted { get; init; }

    /// <summary>
    /// Objekt je použit pouze v případě varianty nemovitosti HF+F Byt.
    /// </summary>
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public FlatOnlyDetailsDto? FlatOnlyDetails { get; init; }

    /// <summary>
    /// Objekt je použit pouze v případě, že jde o variantu nemovitosti HF (Dům a byt) nebo HF+F (Byt) a že je nemovitost ve stavu "Dokončená".
    /// </summary>
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public FinishedHouseAndFlatDetailsDto? FinishedHouseAndFlatDetails { get; init; }

    public class FlatOnlyDetailsDto
    {
        /// <summary>
        /// True pokud je byt v rodinném, jiném než bytovém domě
        /// </summary>
        [JsonRequired]
        public bool SpecialPlacement { get; init; }

        /// <summary>
        /// True pokud jde o umístění v suterénu
        /// </summary>
        [JsonRequired]
        public bool Basement { get; init; }
    }

    public class FinishedHouseAndFlatDetailsDto
    {
        /// <summary>
        /// True pokud je nemovitost pronajata
        /// </summary>
        [JsonRequired]
        public bool Leased { get; init; }

        /// <summary>
        /// Obecná pronajímatelnost
        /// </summary>
        [JsonRequired]
        public bool LeaseApplicable { get; init; }
    }
}