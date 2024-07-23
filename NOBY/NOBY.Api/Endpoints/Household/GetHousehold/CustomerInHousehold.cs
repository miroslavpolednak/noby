namespace NOBY.Api.Endpoints.Household.GetHousehold;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

public class CustomerInHousehold
    : SharedDto.BaseCustomer
{
    public int? MaritalStatusId { get; set; }

    /// <summary>
    /// Prijmy customera
    /// </summary>
    public List<IncomeBaseData>? Incomes { get; set; }

    /// <summary>
    /// Role klienta
    /// </summary>
    /// <example>1</example>
    public int RoleId { get; set; }

    /// <summary>
    /// Příznak zamknutí příjmů daného CustomerOnSA
    /// </summary>
    public bool LockedIncome { get; set; }

    public DateTime? LockedIncomeDateTime { get; set; }

    public bool IsIdentificationRequested => !Identities?.Any() ?? true;

    /// <summary>
    /// Identity klienta v KB nebo MP
    /// </summary>
    public List<SharedTypes.Types.CustomerIdentity>? Identities { get; set; }

    /// <summary>
    /// Zavazky customera
    /// </summary>
    public List<CustomerObligationObligationFull>? Obligations { get; set; }
}

public class IncomeBaseData
{
    /// <summary>
    /// Celkova castka prijmu
    /// </summary>
    /// <example>25000</example>
    public decimal? Sum { get; set; }

    /// <summary>
    /// Kod meny
    /// </summary>
    /// <example>CZK</example>
    public string? CurrencyCode { get; set; }

    /// <summary>
    /// ID prijmu, pokud se jedna o jiz ulozeny prijem. NULL pokud se jedna o novy prijem.
    /// </summary>
    public int? IncomeId { get; set; }

    public string? IncomeSource { get; set; }

    public bool? HasProofOfIncome { get; set; }

    /// <summary>
    /// Typ prijmu
    /// </summary>
    /// <example>1</example>
    public SharedTypes.Enums.EnumIncomeTypes IncomeTypeId { get; set; }
}