﻿namespace NOBY.Api.Endpoints.Household.SharedDto;

public abstract class BaseCustomer
{
    /// <summary>
    /// ID entity pokud jiz existuje
    /// </summary>
    public int? CustomerOnSAId { get; set; }

    /// <summary>
    /// Jmeno klienta, pokud se ma zalozit novy CustomerOnSA
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Prijmeni klienta, pokud se ma zalozit novy CustomerOnSA
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Datum narozeni klienta, pokud se ma zalozit novy CustomerOnSA
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
}