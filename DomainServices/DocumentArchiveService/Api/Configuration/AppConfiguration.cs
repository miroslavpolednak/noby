using ExternalServices.Sdf.Configuration;
using ExternalServicesTcp.Configuration;
using System.ComponentModel.DataAnnotations;

namespace DomainServices.DocumentArchiveService.Api;

internal sealed class AppConfiguration
{
    public const string SectionName = "AppConfiguration";

    /// <summary>
    /// Nastaveni mapovani technickych uzivatelu sluzby vs. Login v generovanem ID
    /// [service_user, C4M ItChannel]
    /// </summary>
    [Required]
    public Dictionary<string, string>? ServiceUser2LoginBinding { get; set; }

    [Required]
    public SdfConfiguration Sdf { get; set; } = null!;

    [Required]
    public TcpConfiguration Tcp { get; set; } = null!;
}
