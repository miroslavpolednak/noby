using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Api.ExternalServices.MpHome.MpHomeWrapper;
using Grpc.Core;
using System.Text;

namespace DomainServices.CustomerService.Api.ExternalServices.MpHome;

internal class RealMpHomeClient : IMpHomeClient
{
    private readonly ILogger<RealMpHomeClient> _logger;
    private readonly AppConfiguration _configuration;

    public RealMpHomeClient(AppConfiguration configuration, ILogger<RealMpHomeClient> logger)
    {
        _logger = logger;
        _configuration = configuration;
    }

    private HttpClient CreateHttpClient()
            => CreateHttpClient(null);

    private HttpClient CreateHttpClient(HttpMessageHandler? handler)
    {
        var client = handler == null ? new HttpClient() : new HttpClient(handler);

        if (_configuration.MpHome != null && !string.IsNullOrEmpty(_configuration.MpHome.Username) && !string.IsNullOrEmpty(_configuration.MpHome.Password))
        {
            var byteArray = Encoding.ASCII.GetBytes($"{_configuration.MpHome.Username}:{_configuration.MpHome.Password}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }        

        return client;
    }

    private Client CreateClient(HttpClient httpClient)
        => new Client(_configuration.MpHome.ServiceUrl, httpClient);

    private async Task WithClient(Func<Client, Task> fce)
    {
        try
        {
            using (var httpClient = CreateHttpClient())
            {
                await fce(CreateClient(httpClient));
            }
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Aborted, $"chyba", 10009);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Aborted, $"chyba", 10009);
        }
    }

    private async Task<T> WithClient<T>(Func<Client, Task<T>> fce)
    {
        try
        {
            using (var httpClient = CreateHttpClient())
            {
                return await fce(CreateClient(httpClient));
            }
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Aborted, $"chyba", 10000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Aborted, $"chyba", 10000);
        }
    }

    async Task IMpHomeClient.Create(PartnerRequest partner, int partnerId)
    {
        await WithClient(async c => {
            await c.FomsUpdatePartnerAsync(partnerId, partner);
        });
    }

    async Task IMpHomeClient.UpdateBaseData(PartnerBaseRequest partner, int partnerId)
    {
        await WithClient(async c => {
            await c.FomsUpdatePartner2Async(partnerId, partner);
        });
    }

    async Task<int> IMpHomeClient.CreateContact(ContactData contact, int partnerId)
    {
        return await WithClient(async c => {
            var result = await c.FomsCreateContactAsync(partnerId, contact);
            return (int)result.ContactId;
        });
    }

    async Task IMpHomeClient.DeleteContact(int contactId, int partnerId)
    {
        await WithClient(async c => {
            await c.FomsDeleteContactAsync(partnerId, contactId);
        });
    }

    async Task IMpHomeClient.UpdateAddress(AddressData address, int partnerId)
    {
        await WithClient(async c => {
            await c.FomsUpdateAddressAsync(partnerId, address);
        });
    }

    async Task IMpHomeClient.UpdateIdentificationDocument(IdentificationDocument identificationDocument, int partnerId)
    {
        await WithClient(async c => {
            await c.FomsUpdateIdentificationDocumentAsync(partnerId, identificationDocument);
        });
    }
}
