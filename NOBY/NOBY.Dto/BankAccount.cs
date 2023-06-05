namespace NOBY.Dto;

public sealed class BankAccount
{
    /// <summary>
    /// Předčíslí účtu
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Číslo účtu
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Kód banky
    /// </summary>
    public string? BankCode { get; set; }
}
