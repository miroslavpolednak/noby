using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Offer.Dto;

public class CreditWorthinessSimpleInputs
{
    /// <summary>
    /// Sekce aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Celkové měsíční příjmy
    /// </summary>
    public decimal? TotalMonthlyIncome { get; set; }

    /// <summary>
    /// Celkové měsíční příjmy
    /// </summary>
    public decimal? ExpensesRent { get; set; }

    /// <summary>
    /// Ostatní měsíční výdaje
    /// </summary>
    public decimal? ExpensesOther { get; set; }

    /// <summary>
    /// Splátky úvěrů a hypoték
    /// </summary>
    public decimal? LoansInstallmentsAmount { get; set; }

    /// <summary>
    /// Limity kreditních karet
    /// </summary>
    public decimal? CreditCardsAmount { get; set; }

    /// <summary>
    /// Limity debetů
    /// </summary>
    public decimal? AuthorizedOverdraftsTotalAmount { get; set; }

    /// <summary>
    /// Počet dětí
    /// </summary>
    public int? ChildrenCount { get; set; }
}