namespace FOMS.Api.Endpoints.SalesArrangement.Dto;

public sealed class ParametersDrawing
{
    /// <summary>
    /// Žadatel o čerpání
    /// </summary>
    public CIS.Foms.Types.CustomerIdentity? Applicant { get; set; }

    /// <summary>
    /// Zmocněná osoba
    /// </summary>
    public ParametersDrawingAgent? Agent { get; set; }

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
    public DateTime DrawingDate { get; set; }

    /// <summary>
    /// Čerpání bezodkladně
    /// </summary>
    public bool IsImmediateDrawing { get; set; }
}

public sealed class ParametersDrawingAgent
{
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
    public ParametersDrawingDocument? IdentificationDocument { get; set; }
}

public sealed class ParametersDrawingPayout
{
    /// <summary>
    /// Pořadové číslo výplaty v kolekci
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Výše čerpané částky
    /// </summary>
    public decimal DrawingAmount { get; set; }

    /// <summary>
    /// Předčíslí účtu
    /// </summary>
    public string? PrefixAccount { get; set; }

    /// <summary>
    /// Číslo účtu
    /// </summary>
    public string? AccountNumber { get; set; }

    /// <summary>
    /// Kód banky
    /// </summary>
    public string? BankCode { get; set; }

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

public sealed class ParametersDrawingDocument
{
    /// <summary>
    /// Typ osobního dokladu - číselník IdentificationDocumentType - (CIS_TYPY_DOKLADOV)
    /// </summary>
    public int IdentificationDocumentTypeId { get; set; }

    /// <summary>
    /// Číslo osobního dokladu
    /// </summary>
    public string Number { get; set; } = String.Empty;
}

/// <summary>
/// Účet pro splácení
/// </summary>
public sealed class ParametersDrawingRepaymentAccount
{
    public bool IsAccountNumberMissing { get; set; }

    /// <summary>
    /// Předčíslí účtu pro splácení
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Číslo účtu pro splácení
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Kód banky účtu pro splácení
    /// </summary>
    public string? BankCode { get; set; }
}