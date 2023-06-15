using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace DomainServices.CustomerService.ExternalServices.Contacts.V1;

public class RealContactClient : IContactClient
{
    public async Task<Contracts.ValidateContactResponse> ValidatePhone(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_httpClient.BaseAddress}/public/v1/contacts/validate-phone";
        var phone = new Contracts.Phone { PhoneNumber = phoneNumber };
        var response = await _httpClient.PostAsJsonAsync(requestUri, phone, _serializerOptions, cancellationToken);

        return await Common.Helpers.ProcessResponse<Contracts.ValidateContactResponse>(StartupExtensions.ServiceName, response, cancellationToken);
    }

    public async Task<Contracts.ValidateContactResponse> ValidateEmail(string emailAddress, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_httpClient.BaseAddress}/public/v1/contacts/validate-email";
        var email = new Contracts.Email { EmailAddress = emailAddress };
        var response = await _httpClient.PostAsJsonAsync(requestUri, email, _serializerOptions, cancellationToken);

        return await Common.Helpers.ProcessResponse<Contracts.ValidateContactResponse>(StartupExtensions.ServiceName, response, cancellationToken);
    }
    
    
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    public RealContactClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
    }
}