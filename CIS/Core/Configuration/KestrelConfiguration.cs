namespace CIS.Core.Configuration;

/// <summary>
/// Nastavení Kestrel serveru v gRPC službách.
/// </summary>
/// <remarks>
/// Nepoužíváme standardní Kestrel konfigurační soubor, protože nám neumožňuje zadávat např. SSL certifikáty.
/// </remarks>
public sealed class KestrelConfiguration
{
    /// <summary>
    /// Nastavené endpointy pro danou službu
    /// </summary>
    public List<EndpointInfo>? Endpoints { get; set; }

    /// <summary>
    /// SSL certifikát použitý pro vytvoření HTTPS tunelu
    /// </summary>
    public CertificateInfo? Certificate {  get; set; }

    /// <summary>
    /// Nastavení endpointu
    /// </summary>
    public sealed class EndpointInfo
    {
        /// <summary>
        /// Port na kterém endpoint poslouchá
        /// </summary>
        /// <example>30000</example>
        public int Port {  get; set; }

        /// <summary>
        /// Druh protokolu použitý pro daný endpoint
        /// </summary>
        /// <remarks>
        /// 1 = HTTP 1.1
        /// 2 = HTTP 2
        /// </remarks>
        public int Protocol {  get; set; }
    }

    /// <summary>
    /// Nastavení SSL certifikátu
    /// </summary>
    public sealed class CertificateInfo
    {
        /// <summary>
        /// Druh úložiště certifkátu
        /// </summary>
        public LocationTypes Location { get; set; }

        /// <summary>
        /// URI souboru s certifikátem na filesystému
        /// </summary>
        /// <remarks>
        /// Pouze pokud Location=FileSystem
        /// </remarks>
        public string? Path { get; set; }

        /// <summary>
        /// Heslo certifikátu uloženého na filesystému
        /// </summary>
        /// <remarks>
        /// Pouze pokud Location=FileSystem
        /// </remarks>
        public string? Password {  get; set; }

        /// <summary>
        /// Název složky ve Windows Certificate store
        /// </summary>
        /// <example>My, Root</example>
        /// <remarks>
        /// Pouze pokud Location=CertStore
        /// </remarks>
        public System.Security.Cryptography.X509Certificates.StoreName CertStoreName { get; set; }

        /// <summary>
        /// Typ Windows Certificate store
        /// </summary>
        /// <example>LocalMachine, CurrenUser</example>
        /// <remarks>
        /// Pouze pokud Location=CertStore
        /// </remarks>
        public System.Security.Cryptography.X509Certificates.StoreLocation CertStoreLocation { get; set; }

        /// <summary>
        /// Thumbprint certifikátu
        /// </summary>
        /// <remarks>
        /// Pouze pokud Location=CertStore
        /// </remarks>
        public string? Thumbprint { get; set; }

        /// <summary>
        /// Možné způsoby 
        /// </summary>
        public enum LocationTypes
        {
            /// <summary>
            /// Certifikát je uložený na filesystému
            /// </summary>
            FileSystem = 1,

            /// <summary>
            /// Certifikát je uložený ve Windows Certificate store
            /// </summary>
            CertStore = 2
        }
    }
}
