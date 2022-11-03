namespace NOBY.Api.Endpoints.CustomerIncome.Dto;

public abstract class BaseIncome
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
}
