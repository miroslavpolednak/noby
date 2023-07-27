namespace ExternalServices.AddressWhisperer.Dto;

public class AddressDetail
{
    /// <summary>
    /// Dodatek ulice, např. Recepce 2. patro
    /// </summary>
    public string? DeliveryDetails { get; set; }

    /// <summary>
    /// Ulice - AddressWhispererBEService|street
    /// </summary>
    public string? Street { get; set; }

    /// <summary>
    /// Číslo orientační - AddressWhispererBEService|streetNumber
    /// </summary>
    public string? StreetNumber { get; set; }

    /// <summary>
    /// Číslo popisné - AddressWhispererBEService|landRegisterNumber
    /// </summary>
    public string? HouseNumber { get; set; }

    /// <summary>
    /// Číslo evidenční - AddressWhispererBEService|evidenceNumber
    /// </summary>
    public string? EvidenceNumber { get; set; }

    /// <summary>
    /// PSČ - AddressWhispererBEService|postCode
    /// </summary>
    public string? Postcode { get; set; }

    /// <summary>
    /// Město - AddressWhispererBEService|city. Pokud regEx 'Praha ([1-2][0-9]|[1-9])' tak hodnota 'Praha'
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Stát - AddressWhispererBEService|country
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Název části obce - AddressWhispererBEService|cityDistrict
    /// </summary>
    public string? CityDistrict { get; set; }

    /// <summary>
    /// Obvod Prahy - AddressWhispererBEService|city pokud regEx 'Praha ([1-2][0-9]|[1-9])', jinak null
    /// </summary>
    public string? PragueDistrict { get; set; }

    /// <summary>
    /// Id RUIAN adresního bodu - AddressWhispererBEService|id
    /// </summary>
    public string? AddressPointId { get; set; }

    /// <summary>
    /// KATUZ ID, pětimístné číslo katastrálního území, Id RUIAN katastrálního území - AddressWhispererBEService|cadastralAreaId
    /// </summary>
    public int? KatuzId { get; set; }

    /// <summary>
    /// KATUZ ID, pětimístné číslo katastrálního území, Id RUIAN katastrálního území - AddressWhispererBEService|cadastralAreaId
    /// </summary>
    public string? KatuzTitle { get; set; }
}
