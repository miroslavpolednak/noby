namespace CIS.Core.Configuration;

public sealed class KestrelConfiguration
{
    public List<EndpointInfo>? Endpoints { get; set; }

    /// <summary>
    /// Certifikat pouzity pro vytvoreni SSL
    /// </summary>
    public CertificateInfo? Certificate {  get; set; }

    public sealed class EndpointInfo
    {
        public int Port {  get; set; }

        /// <summary>
        /// 1 = HTTP1.1
        /// 2 = HTTP2
        /// </summary>
        public int Protocol {  get; set; }
    }

    public sealed class CertificateInfo
    {
        /// <summary>
        /// Typ ulozeni certifikatu
        /// </summary>
        public LocationTypes Location { get; set; }

        /// <summary>
        /// URI souboru s certifikatem
        /// </summary>
        /// <remarks>
        /// Pokud Location=FileSystem
        /// </remarks>
        public string? Path { get; set; }

        /// <summary>
        /// Heslo k certifikatu
        /// </summary>
        /// <remarks>
        /// Pokud Location=FileSystem
        /// </remarks>
        public string? Password {  get; set; }

        /// <summary>
        /// Nazev slozky v certstore
        /// </summary>
        /// <example>
        /// My, Root...
        /// </example>
        /// <remarks>
        /// Pokud Location=CertStore
        /// </remarks>
        public System.Security.Cryptography.X509Certificates.StoreName CertStoreName { get; set; }

        /// <summary>
        /// Typ certstore
        /// </summary>
        /// <example>
        /// LocalMachine, CurrenUser
        /// </example>
        /// <remarks>
        /// Pokud Location=CertStore
        /// </remarks>
        public System.Security.Cryptography.X509Certificates.StoreLocation CertStoreLocation { get; set; }

        /// <summary>
        /// Thumbprint certifikatu
        /// </summary>
        /// <remarks>
        /// Pokud Location=CertStore
        /// </remarks>
        public string? Thumbprint { get; set; }

        public enum LocationTypes
        {
            Unknown = 0,

            /// <summary>
            /// Certifikat je ulozeny jako soubor na filesystemu
            /// </summary>
            FileSystem = 1,

            /// <summary>
            /// Certifikat je ulozeny ve Win certstoru
            /// </summary>
            CertStore = 2
        }
    }
}
