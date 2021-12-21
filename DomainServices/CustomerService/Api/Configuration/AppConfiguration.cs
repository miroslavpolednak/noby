
using ExternalServices.Eas;
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
    public EasConfiguration EAS { get; set; } = new EasConfiguration();

    /// <summary>
    /// Konfigurace MpHome sluzby
    /// </summary>
    public MpHomeConfiguration MpHome { get; set; } = new MpHomeConfiguration();
}
