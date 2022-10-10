﻿using System.Text.Json.Serialization;

namespace CIS.Foms.Types;

public class Address
{
    [JsonIgnore]
    public bool IsPrimary { get; set; }

    public int? AddressTypeId { get; set; }

    /// <summary>
    /// Ulice
    /// </summary>
    public string? Street { get; set; }

    /// <summary>
    /// Číslo orientační
    /// </summary>
    public string? BuildingIdentificationNumber { get; set; }

    /// <summary>
    /// Číslo popisné
    /// </summary>
    public string? LandRegistryNumber { get; set; }

    /// <summary>
    /// Číslo evidenční
    /// </summary>
    public string? EvidenceNumber { get; set; }

    /// <summary>
    /// PSČ
    /// </summary>
    public string? Postcode { get; set; }

    /// <summary>
    /// Město/Obec
    /// </summary>
    public string? City { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public int? CountryId { get; set; }

    /// <summary>
    /// Část obce
    /// </summary>
    public string? CityDistrict { get; set; }

    public string? DeliveryDetails { get; set; }

    /// <summary>
    /// Praha obvod
    /// </summary>
    public string? PragueDistrict { get; set; }

    [JsonIgnore]
    public string? CountrySubdivision { get; set; }

    [JsonIgnore]
    public DateTime? PrimaryAddressFrom { get; set; }

    [JsonIgnore]
    public string? AddressPointId { get; set; }

    public override bool Equals(object? obj)
    {
        var address2 = obj as Address;

        if (stringCompare(this.Street, address2?.Street)
            && stringCompare(this.City, address2?.City)
            && stringCompare(this.BuildingIdentificationNumber, address2?.BuildingIdentificationNumber)
            && stringCompare(this.LandRegistryNumber, address2?.LandRegistryNumber))
            return true;
        else
            return false;

        bool stringCompare(string? s1, string? s2)
            => (s1 ?? "").Equals(s2, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(AddressTypeId);
        hash.Add(Street);
        hash.Add(BuildingIdentificationNumber);
        hash.Add(LandRegistryNumber);
        hash.Add(Postcode);
        hash.Add(City);
        hash.Add(CountryId);
        return hash.ToHashCode();
    }
}
