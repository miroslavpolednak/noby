
using ExternalServices.MpHome;

namespace DomainServices.CustomerService.Api;

internal sealed class AppConfiguration
{
    /// <summary>
    /// Nastaveni gRPC
    /// </summary>
    //public CIS.Infrastructure.gRPC.GrpcConfiguration? Grpc { get; set; }

    /// <summary>
    /// Konfigurace Eas sluzby
    /// </summary>
    public EASConfiguration EAS { get; set; } = new EASConfiguration();

    public class EASConfiguration
    {
        public CIS.Core.ServiceImplementationTypes Implementation { get; set; }

        /// <summary>
        /// URL endpointu
        /// </summary>
        public string ServiceUrl { get; set; } = string.Empty;
    };

    /// <summary>
    /// Konfigurace MpHome sluzby
    /// </summary>
    public MpHomeConfiguration MpHome { get; set; } = new MpHomeConfiguration();

    //public class MpHomeConfiguration
    //{
    //    public CIS.Core.ServiceImplementationTypes Implementation { get; set; }

    //    /// <summary>
    //    /// URL endpointu
    //    /// </summary>
    //    public string ServiceUrl { get; set; } = string.Empty;

    //    public string Username { get; set; } = string.Empty;

    //    public string Password { get; set; } = string.Empty;
    //}
}
