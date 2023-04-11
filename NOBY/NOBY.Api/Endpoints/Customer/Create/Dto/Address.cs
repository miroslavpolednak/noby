using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Customer.Create.Dto;

/// <summary>
/// Adresa - bydliste, kontaktni atd.
/// </summary>
public class Address
{
    [JsonIgnore]
    public bool IsPrimary { get; set; }

    public int? AddressTypeId { get; set; }

    /// <summary>
    /// Ulice
    /// </summary>
    [Required]
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Číslo orientační
    /// </summary>
    public string StreetNumber { get; set; } = string.Empty;

    /// <summary>
    /// Číslo popisné
    /// </summary>
    public string HouseNumber { get; set; } = string.Empty;

    /// <summary>
    /// Číslo evidenční
    /// </summary>
    public string EvidenceNumber { get; set; } = string.Empty;

    /// <summary>
    /// PSČ
    /// </summary>
    [Required]
    public string Postcode { get; set; } = string.Empty;

    /// <summary>
    /// Město/Obec
    /// </summary>
    [Required]
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int CountryId { get; set; }

    /// <summary>
    /// Část obce
    /// </summary>
    public string CityDistrict { get; set; } = string.Empty;

    public string? DeliveryDetails { get; set; }

    /// <summary>
    /// Praha obvod
    /// </summary>
    public string? PragueDistrict { get; set; }

    /// <summary>
    /// Název územního celku
    /// </summary>
    public string CountrySubdivision { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime PrimaryAddressFrom { get; set; }

    /// <summary>
    /// Id adresního bodu
    /// </summary>
    public string AddressPointId { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        var address2 = obj as CIS.Foms.Types.Address;

        if (stringCompare(this.Street, address2?.Street)
            && stringCompare(this.City, address2?.City)
            && stringCompare(this.StreetNumber, address2?.StreetNumber)
            && stringCompare(this.HouseNumber, address2?.HouseNumber))
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
        hash.Add(StreetNumber);
        hash.Add(HouseNumber);
        hash.Add(Postcode);
        hash.Add(City);
        hash.Add(CountryId);
        return hash.ToHashCode();
    }
}