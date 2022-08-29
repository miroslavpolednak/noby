
using DomainServices.CustomerService.Api.Configuration;
using ExternalServices.Eas;
using ExternalServices.MpHome;
using ExternalServices.CustomerManagement;

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

    /// <summary>
    /// Konfigurace CustomerManagement sluzby
    /// </summary>
    public CMConfiguration CustomerManagement { get; set; } = new CMConfiguration();

    public CustomerManagementConfiguration CustomerManagement2 { get; set; } = null!;
}
