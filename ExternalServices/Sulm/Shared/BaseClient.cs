﻿namespace ExternalServices.Sulm.Shared;

internal abstract class BaseClient<TClient>
    where TClient : class
{
    public Versions Version { get; private init; }

    protected readonly ILogger<TClient> _logger;
    protected readonly SulmConfiguration _configuration;

    public BaseClient(Versions version, SulmConfiguration configuration, ILogger<TClient> logger)
    {
        Version = version;
        _logger = logger;
        _configuration = configuration;
    }

    protected BasicHttpBinding createHttpBinding()
    {
        var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

        basicHttpBinding.SendTimeout = TimeSpan.FromSeconds(5);
        basicHttpBinding.CloseTimeout = TimeSpan.FromSeconds(5);
        basicHttpBinding.MaxReceivedMessageSize = 1500000;
        basicHttpBinding.ReaderQuotas.MaxArrayLength = 1500000;

        return basicHttpBinding;
    }

    protected EndpointAddress createEndpoint()
        => new EndpointAddress(new Uri(_configuration.ServiceUrl));

    protected async Task<IServiceCallResult> callMethod(Func<Task<IServiceCallResult>> fce)
    {
        try
        {
            return await fce();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, e.Message);
            return new ErrorServiceCallResult(9100, $"SULM Endpoint '{_configuration.ServiceUrl}' unavailable");
        }
        catch (EndpointNotFoundException)
        {
            _logger.LogError("SULM Endpoint '{uri}' not found", _configuration.ServiceUrl);
            return new ErrorServiceCallResult(9101, $"SULM Endpoint '{_configuration.ServiceUrl}' not found");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}
