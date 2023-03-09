namespace NOBY.Infrastructure.Configuration;

public class AppConfiguration
{
    /// <summary>
    /// Nastaveni autentizace uzivatele.
    /// </summary>
    public AppConfigurationSecurity? Security { get; set; }

    /// <summary>
    /// When set to false, Swagger middleware is not added to pipeline.
    /// </summary>
    public bool EnableSwaggerUi { get; set; }

    /// <summary>
    /// Folder where temp files gonna be stored  
    /// </summary>
    public string FileTempFolderLocation { get; set; } = Path.Combine(Path.GetTempPath(), "Noby");
}
