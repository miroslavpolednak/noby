namespace NOBY.Dto;

public class BankAccount
    : SharedTypes.Interfaces.IBankAccount
{
    /// <summary>
    /// Předčíslí účtu
    /// </summary>
    public string? AccountPrefix { get; set; }

    /// <summary>
    /// Číslo účtu
    /// </summary>
    public string? AccountNumber { get; set; }

    /// <summary>
    /// Kód banky
    /// </summary>
    public string? AccountBankCode { get; set; }
}
