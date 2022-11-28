using System.ComponentModel.DataAnnotations;

namespace ExternalServices.Sdf.Configuration
{
    public sealed class SdfConfiguration
    {
        public const string SectionName = "Sdf";

        public Versions Version { get; set; } = Versions.Unknown;

        [Required]
        public string ServiceUrl { get; set; } = null!;

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string Timeout { get; set; } = "00:01:00";

        public bool EnableSoapMessageLogging { get; set; }

    }
}
