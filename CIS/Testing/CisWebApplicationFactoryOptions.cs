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

    /// <summary>
    /// Create mock of appsettings. If enabled, test project have to has appsettings.Testing.json 
    /// </summary>
    public bool UseTestAppsettings { get; set; } = true;

    /// <summary>
    /// Create mock of CisEnvironmentConfiguration via configuration in appsettings.Testing.json. 
    /// If we want disable service discovery, we have to set: "CisEnvironmentConfiguration"__"DisableServiceDiscovery": true 
    /// </summary>
    public bool UseMockCisEnvironmentConfiguration { get; set; } = true;

    public string AppsettingsName { get; set; } = "appsettings.Testing.json";
}
