namespace NOBY.Infrastructure.Configuration;

public sealed class AppConfiguration
{
    /// <summary>
    /// Nastaveni autentizace uzivatele.
    /// </summary>
    public AppConfigurationSecurity? Security { get; set; }

    /// <summary>
    /// Pokud je nastaveno na true, vynecha se exception middleware a zobrazuje se detailni stranka s popisem chyby.
    /// </summary>
    public bool UseDeveloperExceptionPage { get; set; }

    /// <summary>
    /// Folder where temp files gonna be stored  
    /// </summary>
    public string FileTempFolderLocation { get; set; } = Path.Combine(Path.GetTempPath(), "Noby");

    /// <summary>
    /// Max allowed file size for upload [MB]
    /// </summary>
    public int MaxFileSize { get; set; } = 20;

    /// <summary>
    /// ID prostredi pro ktere se ma nahrat config pro MPSS.Security.dll
    /// </summary>
    public int? MpssSecurityDllEnvironment { get; set; }

    public IcapAntivirusConfiguration? IcapAntivirus { get; set; }

    public sealed class IcapAntivirusConfiguration
    {
        public string ServerAddress { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
