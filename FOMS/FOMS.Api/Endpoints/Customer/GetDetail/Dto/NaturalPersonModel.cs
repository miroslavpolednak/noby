﻿namespace FOMS.Api.Endpoints.Customer.GetDetail.Dto;

public class NaturalPersonModel
{
    /// <summary>
    /// Rodné číslo CZ
    /// </summary>
    public string? BirthNumber { get; set; }

    /// <summary>
    /// Jméno
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Příjmení
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Datum narození
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    public int? DegreeBeforeId  { get; set; }
    public int? DegreeAfterId  { get; set; }

    public string? BirthName  { get; set; }

    /// <summary>
    /// Místo narození
    /// </summary>
    public string? PlaceOfBirth { get; set; }

    public int? BirthCountryId  { get; set; }

    /// <summary>
    /// Pohlaví
    /// </summary>
    public CIS.Foms.Enums.Genders Gender { get; set; }

    public int? MaritalStatusStateId { get; set; }

    /// <summary>
    /// Státní příslušnost/občanství
    /// </summary>
    public List<int>? CitizenshipCountriesId { get; set; }

    public bool? IsBrSubscribed { get; set; }
}