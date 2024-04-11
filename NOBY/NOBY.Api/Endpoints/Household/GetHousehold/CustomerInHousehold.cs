﻿namespace NOBY.Api.Endpoints.Household.GetHousehold;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

public class CustomerInHousehold
    : SharedDto.BaseCustomer
{
    public int? MaritalStatusId { get; set; }

    /// <summary>
    /// Prijmy customera
    /// </summary>
    public List<CustomerIncome.SharedDto.IncomeBaseData>? Incomes { get; set; }

    /// <summary>
    /// Role klienta
    /// </summary>
    /// <example>1</example>
    public int RoleId { get; set; }

    public DateTime? LockedIncomeDateTime { get; set; }

    public bool IsIdentificationRequested => !Identities?.Any() ?? true;

    /// <summary>
    /// Identity klienta v KB nebo MP
    /// </summary>
    public List<SharedTypes.Types.CustomerIdentity>? Identities { get; set; }

    /// <summary>
    /// Zavazky customera
    /// </summary>
    public List<CustomerObligation.SharedDto.ObligationFullDto>? Obligations { get; set; }
}