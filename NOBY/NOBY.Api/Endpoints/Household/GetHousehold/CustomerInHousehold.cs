namespace NOBY.Api.Endpoints.Household.GetHousehold;

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

    /// <summary>
    /// Identity klienta v KB nebo MP
    /// </summary>
    public List<SharedTypes.Types.CustomerIdentity>? Identities { get; set; }

    /// <summary>
    /// Zavazky customera
    /// </summary>
    public List<CustomerObligation.SharedDto.ObligationFullDto>? Obligations { get; set; }
}