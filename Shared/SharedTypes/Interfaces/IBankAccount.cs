namespace SharedTypes.Interfaces;

public interface IBankAccount
{
    /// <summary>
    /// Předčíslí účtu
    /// </summary>
    string? AccountPrefix { get; set; }

    /// <summary>
    /// Číslo účtu
    /// </summary>
    string? AccountNumber { get; set; }

    /// <summary>
    /// Kód banky
    /// </summary>
    string? AccountBankCode { get; set; }
}
