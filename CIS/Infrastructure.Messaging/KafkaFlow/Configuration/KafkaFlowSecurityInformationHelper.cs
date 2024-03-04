using CIS.Infrastructure.Messaging.Configuration;
using KafkaFlow.Configuration;

namespace CIS.Infrastructure.Messaging.KafkaFlow.Configuration;

internal static class KafkaFlowSecurityInformationHelper
{
    public static void SetSecurityInfo(KafkaFlowConfiguration configuration, SecurityInformation securityInfo)
    {
        switch (configuration.SecurityProtocol)
        {
            case Confluent.Kafka.SecurityProtocol.Ssl: 
                SetSslSecurityInfo(configuration, securityInfo);
                break;

            case Confluent.Kafka.SecurityProtocol.Plaintext:
            case Confluent.Kafka.SecurityProtocol.SaslPlaintext:
            case Confluent.Kafka.SecurityProtocol.SaslSsl:
            default:
                SetDefaultSecurityInfo(securityInfo);
                break;
        }
    }

    private static void SetSslSecurityInfo(KafkaFlowConfiguration configuration, SecurityInformation securityInfo)
    {
        securityInfo.EnableSslCertificateVerification = true;
        securityInfo.SecurityProtocol = SecurityProtocol.Ssl;
        securityInfo.SslKeyLocation = configuration.SslKeyLocation;
        securityInfo.SslKeyPassword = configuration.SslKeyPassword;
        securityInfo.SslCertificateLocation = configuration.SslCertificateLocation;
        securityInfo.SslEndpointIdentificationAlgorithm = SslEndpointIdentificationAlgorithm.None;
    }

    private static void SetDefaultSecurityInfo(SecurityInformation securityInfo)
    {
        securityInfo.EnableSslCertificateVerification = false;
    }
}