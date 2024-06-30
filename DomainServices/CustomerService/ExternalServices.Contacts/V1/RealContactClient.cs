using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using DomainServices.CustomerService.ExternalServices.Contacts.V1.Contracts;

namespace DomainServices.CustomerService.ExternalServices.Contacts.V1;

public class RealContactClient : IContactClient
{
    public async Task<ValidateContactResponse> ValidatePhone(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_httpClient.BaseAddress}/public/v1/contacts/validate-phone";
        var phone = new Phone { PhoneNumber = phoneNumber };
        var response = await _httpClient.PostAsJsonAsync(requestUri, phone, _serializerOptions, cancellationToken);

        return await Common.Helpers.ProcessResponse<ValidateContactResponse>(StartupExtensions.ServiceName, response, cancellationToken);
    }

    public async Task<ValidateContactResponse> ValidateEmail(string emailAddress, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_httpClient.BaseAddress}/public/v1/contacts/validate-email";
        var email = new Email { EmailAddress = emailAddress };
        var response = await _httpClient.PostAsJsonAsync(requestUri, email, _serializerOptions, cancellationToken);

        return await Common.Helpers.ProcessResponse<ValidateContactResponse>(StartupExtensions.ServiceName, response, cancellationToken);
    }

    public async Task<List<Contact>> LoadContacts(long customerId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_httpClient.BaseAddress}/public/v1/customers/{customerId}/contacts";
        var response = await _httpClient.GetAsync(requestUri, cancellationToken);

        return await Common.Helpers.ProcessResponse<List<Contact>>(StartupExtensions.ServiceName, response, cancellationToken);
    }

    public async Task CreateOrUpdateContact(long customerId, int contactMethodCode, string contactValue, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_httpClient.BaseAddress}/public/v1/customers/{customerId}/contacts";
        var request = new ContactBase
        {
            ContactMethodCode = contactMethodCode,
            ContactValue = contactValue
        };

        var response = await _httpClient.PutAsJsonAsync(requestUri, request, cancellationToken);

        await Common.Helpers.ProcessResponse(StartupExtensions.ServiceName, response, cancellationToken);
    }

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    public RealContactClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
    }
}