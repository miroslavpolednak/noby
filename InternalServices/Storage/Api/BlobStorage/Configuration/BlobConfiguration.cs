namespace CIS.InternalServices.Storage.Api.BlobStorage;

internal class BlobConfiguration
{
    public Providers Provider { get; set; }

    /// <summary>
    /// Konfigurace LocalFilesystem provideru
    /// </summary>
    public LocalFs? LocalFilesystem { get; set; }

    public enum Providers
    {
        /// <summary>
        /// Ukladani souboru na klasicky NTFS filesystem na stejnem serveru, jako bezi sluzba
        /// </summary>
        LocalFilesystem = 1
    }

    public class LocalFs
    {
        public string? BasePath { get; set; }

        public string? BaseTempPath { get; set; }
    }
}
