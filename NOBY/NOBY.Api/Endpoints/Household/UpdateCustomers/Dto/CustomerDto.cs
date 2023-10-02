﻿namespace NOBY.Api.Endpoints.Household.UpdateCustomers.Dto;

public class CustomerDto 
    : Endpoints.Household.Dto.BaseCustomer
{
    /// <summary>
    /// Příznak zamknutí příjmů daného CustomerOnSA
    /// </summary>
    public bool LockedIncome { get; set; }

    /// <summary>
    /// Identita klienta, pokud se ma zalozit novy CustomerOnSA
    /// </summary>
    public SharedTypes.Types.CustomerIdentity? Identity { get; set; }
}