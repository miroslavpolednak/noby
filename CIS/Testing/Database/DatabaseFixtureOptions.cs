namespace CIS.Testing.Database;

/// <summary>
/// Nastaveni databaze pro testy
/// </summary>
public class DatabaseFixtureOptions
{
    /// <summary>
    /// Cesta k souboru, ktery vytvori strukturu databaze nebo naplni db daty.
    /// Vychozi cesta je {tests base dir}/{test name}/DatabaseSeed.sql
    /// Cesta muze byt i relativni s ~/ na zacatku - pak se vztahuje k base adresari testu.
    /// Souboru muze byt vice (oddelenych ;)
    /// </summary>
    public string? SeedPaths { get; set; }
}
