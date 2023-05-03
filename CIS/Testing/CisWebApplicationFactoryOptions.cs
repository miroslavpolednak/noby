using CIS.Testing.Database;

namespace CIS.Testing;
public class CisWebApplicationFactoryOptions
{
    /// <summary>
    /// Automatically find all dbContexts in project and replace real databases with in memory databases.
    /// If you set this property to false, you have to manually register in memory database, if you don't do it,
    /// regular database gonna be used and this is terrible wrong
    /// </summary>
    public bool UseDbContextAutoMock { get; set; } = true;

    /// <summary>
    /// Replace real logger by NullLogger (NullLogger is a fake logger which logg nothing) 
    /// </summary>
    public bool UseNullLogger { get; set; } = true;

    /// <summary>
    /// For every request noby authentication header is going to be add 
    /// </summary>
    public bool UseNobyAuthenticationHeader { get; set; } = true;

    /// <summary>
    /// If we want some custom header, we can add it via this dictionary  
    /// </summary>
    public Dictionary<string, string?>? Header { get; set; }

    public string AppSettingsName { get; set; } = "appsettings.Testing.json";

    /// <summary>
    /// Currently we have only EfInMemoryMockAdapter and SqliteInMemoryMockAdapter (Default: EfInMemoryMockAdapter).
    /// </summary>
    public IDbMockAdapter DbMockAdapter { get; set; } = new EfInMemoryMockAdapter();
}
