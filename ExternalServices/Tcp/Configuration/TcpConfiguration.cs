using System.ComponentModel.DataAnnotations;

namespace ExternalServicesTcp.Configuration
{
    public class TcpConfiguration
    {
        public const string SectionName = "Tcp";

        [Required]
        public string Connectionstring { get; set; } = null!;
    }
}
