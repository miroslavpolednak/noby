using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using CIS.Infrastructure.ExternalServicesHelpers.Soap;
using CIS.Infrastructure.Logging.Extensions.Extensions;
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
    private readonly ILogger _logger;

    protected SoapClient Client => _client;

    protected IExternalServiceConfiguration Configuration => _configuration;

    public SoapClientBase(
        IExternalServiceConfiguration configuration,
        ILogger logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger;
        _client = new SoapClient();
        _client.Endpoint.Address = new EndpointAddress(_configuration.ServiceUrl);
        _client.Endpoint.Binding = CreateBinding();

        _client.Endpoint.EndpointBehaviors.Add(new MaxFaultSizeBehavior(2147483647));

        if (_configuration.UseLogging)
        {
            _client.Endpoint.SetTraceLogging(logger, new()
            {
                ServiceUrl = _configuration.ServiceUrl!.AbsoluteUri,
                LogRequestPayload = _configuration.LogRequestPayload,
                LogResponsePayload = _configuration.LogResponsePayload
            });
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
    protected abstract string ServiceName { get; }

    protected async Task<TResult> callMethod<TResult>(Func<Task<TResult>> fce)
    {
        try
        {
            return await fce();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new CIS.Core.Exceptions.CisServiceUnavailableException(ServiceName, nameof(callMethod), ex.Message);
        }
        catch (EndpointNotFoundException ex)
        {
            _logger.LogError("Endpoint '{uri}' not found", _configuration.ServiceUrl);
            throw new CIS.Core.Exceptions.CisServiceUnavailableException(ServiceName, nameof(callMethod), ex.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}

