namespace NOBY.Dto;

public class BankAccount
    : IBankAccount
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

public interface IBankAccount
{
    string? AccountPrefix { get; set; }
    string? AccountNumber { get; set; }
    string? AccountBankCode { get; set; }
}