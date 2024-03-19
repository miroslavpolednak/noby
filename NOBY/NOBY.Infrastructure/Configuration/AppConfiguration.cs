namespace NOBY.Infrastructure.Configuration;

public sealed class AppConfiguration
{
    /// <summary>
    /// Zapnuti logovani rozdilu mezi rozdilem a modelem
    /// </summary>
    public bool LogRequestContractDifferences { get; set; }

    /// <summary>
    /// Nastaveni autentizace uzivatele.
    /// </summary>
    public AppConfigurationSecurity? Security { get; set; }

    /// <summary>
    /// Pokud je nastaveno na true, vynecha se exception middleware a zobrazuje se detailni stranka s popisem chyby.
    /// </summary>
    public bool UseDeveloperExceptionPage { get; set; }

    /// <summary>
    /// Max allowed file size for upload [MB]
    /// </summary>
    public int MaxFileSize { get; set; } = 20;

    public bool UseKafkaFlowDashboard { get; set; }

    public IcapAntivirusConfiguration? IcapAntivirus { get; set; }

    public sealed class IcapAntivirusConfiguration
    {
        public string ServerAddress { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
