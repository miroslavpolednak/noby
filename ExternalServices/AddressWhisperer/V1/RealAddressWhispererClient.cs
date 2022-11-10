using CIS.Infrastructure.Logging;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace ExternalServices.AddressWhisperer.V1;

internal class RealAddressWhispererClient
    : IAddressWhispererClient
{
    public async Task<IServiceCallResult> GetAddressDetail(string sessionId, string addressId, string title, string country, CancellationToken cancellationToken)
    {
        string soap = _soapEnvelopeStart + getHeader() + $@"<soapenv:Body>
      <v1:getAddressDetailsReq>
         <dto:sessionId>{sessionId}</dto:sessionId>
         <dto:simplePostalAddressPointRepresentation>
            <dto:country>{country}</dto:country>
         </dto:simplePostalAddressPointRepresentation>
         <dto:suggestedAddress>
            <dto:id>{addressId}</dto:id>
            <dto:title>{title}</dto:title>
         </dto:suggestedAddress>
      </v1:getAddressDetailsReq>
   </soapenv:Body>" + _soapEnvelopeEnd;

        using (HttpContent content = new StringContent(soap, Encoding.UTF8, "text/xml"))
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _configuration.ServiceUrl))
        {
            request.Headers.Add("SOAPAction", "getAddressDetails");
            request.Content = content;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", await request.Content!.ReadAsStringAsync(cancellationToken) }
            }))
            {
                _logger.HttpRequestPayload("getAddressDetails", _configuration.ServiceUrl);
            }

            using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                string rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    { "Payload", rawResponse }
                }))
                {
                    _logger.HttpResponsePayload("getAddressDetails", _configuration.ServiceUrl, (int)response.StatusCode);
                }

                response.EnsureSuccessStatusCode();
                var xml = XElement.Parse(rawResponse);

                var baseNode = xml
                    .Descendants(_soapenv + "Body")
                    .Descendants(_ns1 + "getAddressDetailsRes")
                    .Descendants(_ns2 + "addressPointPostalRepresentationList")
                    .Descendants(_ns2 + "addressPointPostalRepresentation")
                    .Where(t => t.Attribute(_ns3 + "type")?.Value == "nsDto:ComponentAddressPointRepresentation")
                    .FirstOrDefault();

                if (baseNode is not null)
                {
                    return new SuccessfulServiceCallResult<Shared.AddressDetail>(new Shared.AddressDetail
                    {
                        City = baseNode.Element(_ns2 + "city")?.Value,
                        CityDistrict = baseNode.Element(_ns2 + "cityDistrict")?.Value,
                        Country = baseNode.Element(_ns2 + "country")?.Value,
                        HouseNumber = baseNode.Element(_ns2 + "landRegisterNumber")?.Value,
                        Postcode = baseNode.Element(_ns2 + "postcode")?.Value,
                        Street = baseNode.Element(_ns2 + "street")?.Value
                    });
                }
                else
                    return new EmptyServiceCallResult();
            }
        }
    }

    public async Task<IServiceCallResult> GetSuggestions(string sessionId, string text, int pageSize, string? country, CancellationToken cancellationToken)
    {
        string soap = _soapEnvelopeStart + getHeader() + $@"<soapenv:Body>
      <v1:getSuggestionsReq>
         <dto:addressPattern>{text}</dto:addressPattern>
         <dto:sessionId>{sessionId}</dto:sessionId>
         <dto:paging>
            <dto:numberOfEntries>{pageSize}</dto:numberOfEntries>
         </dto:paging>
         <dto:simplePostalAddressPointRepresentation>
            <dto:country>{country ?? "CZ"}</dto:country>
         </dto:simplePostalAddressPointRepresentation>
      </v1:getSuggestionsReq>
   </soapenv:Body>" + _soapEnvelopeEnd;
        
        using (HttpContent content = new StringContent(soap, Encoding.UTF8, "text/xml"))
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _configuration.ServiceUrl))
        {
            request.Headers.Add("SOAPAction", "getSuggestions");
            request.Content = content;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", await request.Content!.ReadAsStringAsync(cancellationToken) }
            }))
            {
                _logger.HttpRequestPayload("getSuggestions", _configuration.ServiceUrl);
            }

            using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                string rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    { "Payload", rawResponse }
                }))
                {
                    _logger.HttpResponsePayload("getSuggestions", _configuration.ServiceUrl, (int)response.StatusCode);
                }

                response.EnsureSuccessStatusCode();
                var xml = XElement.Parse(rawResponse);

                var responseModel = new List<Shared.FoundSuggestion>();
                foreach (var itm in xml
                    .Descendants(_soapenv + "Body")
                    .Descendants(_ns1 + "getSuggestionsRes")
                    .Descendants(_ns2 + "suggestedAddressList")
                    .Descendants(_ns2 + "suggestedAddress"))
                {
                    responseModel.Add(new Shared.FoundSuggestion
                    {
                        AddressId = itm.Element(_ns2 + "id")!.Value,
                        Title = itm.Element(_ns2 + "title")!.Value
                    });
                }

                if (responseModel.Any())
                    return new SuccessfulServiceCallResult<List<Shared.FoundSuggestion>>(responseModel);
                else
                    return new EmptyServiceCallResult();
            }
        }
    }

    XNamespace _soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
    XNamespace _ns1 = "http://kb.cz/AddressWhispererBEService/v1";
    XNamespace _ns2 = "http://kb.cz/AddressWhispererBEService/v1/DTO";
    XNamespace _ns3 = "http://www.w3.org/2001/XMLSchema-instance";
    private const string _soapEnvelopeStart = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:v1=""http://kb.cz/AddressWhispererBEService/v1"" xmlns:v11=""http://kb.cz/DataModel/Technical/HeaderTypes/v1"" xmlns:dto=""http://kb.cz/AddressWhispererBEService/v1/DTO"">";
    private const string _soapEnvelopeEnd = @"</soapenv:Envelope>";

    private string getHeader()
        => $@"<soapenv:Header>
      <wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
         <wsse:UsernameToken wsu:Id=""UsernameToken-{Guid.NewGuid().ToString("N")}"">
            <wsse:Username>{_configuration.Username}</wsse:Username>
            <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">{_configuration.Password}</wsse:Password>
            <wsse:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">EJCe9O91tt/+NVn1JThDWg==</wsse:Nonce>
            <wsu:Created>{DateTime.Now:yyyy-MM-ddTHH:mm:sssZ}</wsu:Created>
         </wsse:UsernameToken>
      </wsse:Security>
     <v1:traceContext>
         <v11:traceId>{Activity.Current?.TraceId.ToHexString() ?? Guid.NewGuid().ToString()}</v11:traceId>
         <v11:timestamp>{DateTime.Now:yyyy-MM-ddTHH:mm:ss}+02:00</v11:timestamp>
      </v1:traceContext>
      <v1:systemIdentity>
         <v11:originator>
            <v11:application>NOBY</v11:application>
            <v11:applicationComponent>NOBY.FE</v11:applicationComponent>
         </v11:originator>
         <v11:caller>
            <v11:application>NOBY</v11:application>
            <v11:applicationComponent>NOBY.FE</v11:applicationComponent>
         </v11:caller>
      </v1:systemIdentity>
   </soapenv:Header>";

    private readonly HttpClient _httpClient;
    private readonly AddressWhispererConfiguration _configuration;
    private readonly ILogger<RealAddressWhispererClient> _logger;

    public RealAddressWhispererClient(HttpClient httpClient, AddressWhispererConfiguration configuration, ILogger<RealAddressWhispererClient> logger)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _logger = logger;
    }
}
