using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace DomainServices.CustomerService.Api.Clients;

[TransientService, SelfService]
public class CertificationValidatorHttpHandler : DelegatingHandler
{
    public CertificationValidatorHttpHandler() : base(new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = ValidationCallback
    })
    {
    }

    private static bool ValidationCallback(HttpRequestMessage arg1, X509Certificate2? arg2, X509Chain? arg3, SslPolicyErrors arg4)
    {
        return true;
    }
}