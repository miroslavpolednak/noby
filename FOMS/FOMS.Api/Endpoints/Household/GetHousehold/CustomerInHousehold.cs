namespace FOMS.Api.Endpoints.Household.GetHousehold;

public class CustomerInHousehold
    : Dto.BaseCustomer
{
    public int? MaritalStatusId { get; set; }

    /// <summary>
    /// Prijmy customera
    /// </summary>
    public List<CustomerIncome.Dto.IncomeBaseData>? Incomes { get; set; }

    /// <summary>
    /// Role klienta
    /// </summary>
    /// <example>1</example>
    public int RoleId { get; set; }

    public DateTime? LockedIncomeDateTime { get; set; }

    /// <summary>
    /// Identity klienta v KB nebo MP
    /// </summary>
    public List<CIS.Foms.Types.CustomerIdentity>? Identities { get; set; }

    /// <summary>
    /// Zavazky customera
    /// </summary>
    public List<CustomerObligation.Dto.ObligationFullDto>? Obligations { get; set; }
}