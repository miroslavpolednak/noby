namespace ExternalServices.Eas.Shared;

internal abstract class BaseClient<TClient>
    where TClient : class
{
    protected readonly CIS.Infrastructure.Telemetry.IAuditLogger _auditLogger;
    protected readonly ILogger<TClient> _logger;
    protected readonly CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration _configuration;

    public BaseClient(CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration configuration, ILogger<TClient> logger, CIS.Infrastructure.Telemetry.IAuditLogger auditLogger)
    {
        _logger = logger;
        _configuration = configuration;
        _auditLogger = auditLogger;
    }

    protected BasicHttpBinding createHttpBinding()
    {
        var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        if (_configuration.RequestTimeout.HasValue)
        {
            basicHttpBinding.SendTimeout = TimeSpan.FromSeconds(_configuration.RequestTimeout.Value);
            basicHttpBinding.CloseTimeout = TimeSpan.FromSeconds(_configuration.RequestTimeout.Value);
        }
        basicHttpBinding.MaxReceivedMessageSize = 1500000;
        basicHttpBinding.ReaderQuotas.MaxArrayLength = 1500000;

        return basicHttpBinding;
    }

    protected EndpointAddress createEndpoint()
        => new EndpointAddress(_configuration.ServiceUrl);

    protected async Task<TResult> callMethod<TResult>(Func<Task<TResult>> fce)
    {
        try
        {
            return await fce();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new CIS.Core.Exceptions.CisServiceUnavailableException(StartupExtensions.ServiceName, nameof(callMethod), ex.Message);
        }
        catch (EndpointNotFoundException ex)
        {
            _logger.LogError("EAS Endpoint '{uri}' not found", _configuration.ServiceUrl);
            throw new CIS.Core.Exceptions.CisServiceUnavailableException(StartupExtensions.ServiceName, nameof(callMethod), ex.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}
