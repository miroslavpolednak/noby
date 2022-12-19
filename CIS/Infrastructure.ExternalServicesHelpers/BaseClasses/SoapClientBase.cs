using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using CIS.Infrastructure.Logging.Extensions;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

namespace CIS.Infrastructure.ExternalServicesHelpers.BaseClasses;
public abstract class SoapClientBase<SoapClient, SoapClientChannel> : IDisposable
    where SoapClient : ClientBase<SoapClientChannel>, new()
    where SoapClientChannel : class
{
    private readonly SoapClient _client;
    private readonly IExternalServiceConfiguration _configuration;

    protected SoapClient Client => _client;

    protected IExternalServiceConfiguration Configuration => _configuration;

    public SoapClientBase(
        IExternalServiceConfiguration configuration,
        ILogger logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        _client = new SoapClient();
        _client.Endpoint.Address = new EndpointAddress(_configuration.ServiceUrl);
        _client.Endpoint.Binding = CreateBinding();

        if (_configuration.LogPayloads)
        {
            _client.Endpoint.SetTraceLogging(logger, _configuration.ServiceName ?? string.Empty);
        }

        if (_configuration.Authentication == ExternalServicesAuthenticationTypes.Basic)
        {
            _client.ClientCredentials.UserName.UserName = _configuration.Username;
            _client.ClientCredentials.UserName.Password = _configuration.Password;
        }

        if (_configuration.IgnoreServerCertificateErrors)
        {
            _client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
            {
                CertificateValidationMode = X509CertificateValidationMode.None,
                RevocationMode = X509RevocationMode.NoCheck
            };
        }
    }

    public void Dispose()
    {
        if (_client.State == CommunicationState.Faulted)
        {
            _client.Abort();
        }
        else
        {
            _client.Close();
        }
    }
    protected abstract Binding CreateBinding();
}
