using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using CIS.Infrastructure.ExternalServicesHelpers.Soap;
using CIS.Infrastructure.Logging.Extensions.Extensions;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

namespace CIS.Infrastructure.ExternalServicesHelpers.BaseClasses;

public abstract class SoapClientBase<TSoapClient, TSoapClientChannel> : IDisposable
    where TSoapClient : ClientBase<TSoapClientChannel>, new()
    where TSoapClientChannel : class
{
    private readonly TSoapClient _client;
    private readonly IExternalServiceConfiguration _configuration;
    protected readonly ILogger Logger;

    protected TSoapClient Client => _client;

    protected IExternalServiceConfiguration Configuration => _configuration;

    public SoapClientBase(
        IExternalServiceConfiguration configuration,
        ILogger logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Logger = logger;
        _client = new TSoapClient();
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
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
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
    }

    protected abstract Binding CreateBinding();
    protected abstract string ServiceName { get; }

    protected async Task<TResult> callMethod<TResult>(Func<Task<TResult>> fce)
    {
        try
        {
            return await fce();
        }
        catch (Exception ex) when (ex is InvalidOperationException || ex is EndpointNotFoundException)
        {
            Logger.ExtServiceUnavailable(ServiceName, ex);
            throw new CisServiceUnavailableException(ServiceName, nameof(callMethod), ex.Message);
        }
        catch (Exception e)
        {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA2254 // Template should be a static expression
            Logger.LogError(e, e.Message);
#pragma warning restore CA2254 // Template should be a static expression
#pragma warning restore CA1848 // Use the LoggerMessage delegates
            throw;
        }
    }
}

