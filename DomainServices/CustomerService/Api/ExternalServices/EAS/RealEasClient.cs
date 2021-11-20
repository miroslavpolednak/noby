using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Api.ExternalServices.EAS.EasWrapper;
using Grpc.Core;
using System.ServiceModel;

namespace DomainServices.CustomerService.Api.ExternalServices.EAS;

internal class RealEasClient : IEasClient
{
    private readonly ILogger<RealEasClient> _logger;
    private readonly AppConfiguration _configuration;

    public RealEasClient(ILogger<RealEasClient> logger, AppConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    private EAS_WS_SB_ServicesClient CreateClient()
    {
        var binding = new BasicHttpBinding
        {
            MaxReceivedMessageSize = 1500000
        };
        binding.ReaderQuotas.MaxArrayLength = 1500000;
        binding.Security.Mode = BasicHttpSecurityMode.Transport;

        return new EAS_WS_SB_ServicesClient(binding, new EndpointAddress(new Uri(_configuration.EAS.ServiceUrl)));
    }

    private async Task<T> WithClient<T>(Func<EAS_WS_SB_ServicesClient, Task<T>> fce)
    {
        try
        {
            using (EAS_WS_SB_ServicesClient client = CreateClient())
            {
                return await fce(client);
            }
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, e.Message);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Aborted, $"EAS Endpoint '{_configuration.EAS.ServiceUrl}' unavailable", 10009);
        }
        catch (System.ServiceModel.EndpointNotFoundException)
        {
            _logger.LogError("EAS Endpoint '{uri}' not found", _configuration.EAS.ServiceUrl);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Aborted, $"EAS Endpoint '{_configuration.EAS.ServiceUrl}' not found", 10008);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<int?> GetKlientData(string birthNumber)
    {
        return await WithClient<int?>(async c => {

            var requestModel = new S_KLIENTDATA() { rodne_cislo_ico = birthNumber };

            var output = await c.GetKlientDataAsync(requestModel);

            return output != null && output.klient_id > 0 ? output.klient_id : null;
        });
    }

    public async Task<S_KLIENTDATA> NewKlient(S_KLIENTDATA client)
    {
        return await WithClient(async c => {

            var output = await c.NewKlientAsync(client);

            return output;
        });
    }
}
