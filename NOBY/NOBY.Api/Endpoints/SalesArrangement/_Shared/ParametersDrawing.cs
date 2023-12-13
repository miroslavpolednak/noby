using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.Dto;

public sealed class ParametersDrawing
{
    /// <summary>
    /// Žadatel o čerpání
    /// </summary>
    public List<SharedTypes.Types.CustomerIdentity>? Applicant { get; set; }

    /// <summary>
    /// Zmocněná osoba
    /// </summary>
    [Required]
    public ParametersDrawingAgent? Agent { get; set; } = new();

    /// <summary>
    /// Účet pro splácení
    /// </summary>
    public ParametersDrawingRepaymentAccount? RepaymentAccount { get; set; }

    /// <summary>
    /// Seznam výplat
    /// </summary>
    public List<ParametersDrawingPayout>? PayoutList { get; set; }

    /// <summary>
    /// Datum čerpání
    /// </summary>
    [Required]
    public DateTime? DrawingDate { get; set; }

    /// <summary>
    /// Čerpání bezodkladně
    /// </summary>
    public bool IsImmediateDrawing { get; set; }
}

public sealed class ParametersDrawingAgent
{
    /// <summary>
    /// Sekce je aktivní
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

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

    /// <summary>
    /// Osobní doklad
    /// </summary>
    public NOBY.Dto.IdentificationDocumentBase? IdentificationDocument { get; set; }
}

public sealed class ParametersDrawingPayout
    : NOBY.Dto.BankAccount
{
    public int? ProductObligationId { get; set; }

    /// <summary>
    /// Pořadové číslo výplaty v kolekci
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Výše čerpané částky
    /// </summary>
    public decimal? DrawingAmount { get; set; }

    /// <summary>
    /// Variabilní symbol
    /// </summary>
    public string? VariableSymbol { get; set; }

    /// <summary>
    /// Specifický symbol
    /// </summary>
    public string? SpecificSymbol { get; set; }

    /// <summary>
    /// Konstantní symbol
    /// </summary>
    public string? ConstantSymbol { get; set; }

    /// <summary>
    /// Typ výplaty - konsolidace nebo výplata
    /// </summary>
    public int? PayoutTypeId { get; set; }
}

/// <summary>
/// Účet pro splácení
/// </summary>
public sealed class ParametersDrawingRepaymentAccount
    : NOBY.Dto.BankAccount
{
    public bool IsAccountNumberMissing { get; set; }
}