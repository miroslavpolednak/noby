using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace ExternalServices.AddressWhisperer.V1;

internal sealed class RealAddressWhispererClient
    : IAddressWhispererClient
{
    public async Task<Dto.AddressDetail?> GetAddressDetail(string sessionId, string addressId, string title, string country, CancellationToken cancellationToken)
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
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress))
        {
            request.Headers.Add("SOAPAction", "getAddressDetails");
            request.Content = content;

            using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                string rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

                response.EnsureSuccessStatusCode();
                var xml = XElement.Parse(rawResponse);

                var nodes = xml
                    .Descendants(_soapenv + "Body")
                    .Descendants(_ns1 + "getAddressDetailsRes")
                    .Descendants(_ns2 + "addressPointPostalRepresentationList")
                    .Descendants(_ns2 + "addressPointPostalRepresentation")
                    .ToList();
                if (nodes is null || nodes.Count == 0)
                    return null;

                XElement? baseNode;

                if (country == "CZ" || country == "SK")
                    baseNode = nodes!.FirstOrDefault(t => t.Attribute(_ns3 + "type")?.Value == "nsDto:SlovakRuianAddressPointRepresentation");
                else
                    baseNode = nodes!.FirstOrDefault(t => t.Attribute(_ns3 + "type")?.Value == "nsDto:ComponentAddressPointRepresentation");

                if (baseNode is not null)
                {
                    return new Dto.AddressDetail
                    {
                        City = baseNode.Element(_ns2 + "city")?.Value,
                        CityDistrict = baseNode.Element(_ns2 + "cityDistrict")?.Value,
                        Country = baseNode.Element(_ns2 + "country")?.Value,
                        HouseNumber = baseNode.Element(_ns2 + "landRegisterNumber")?.Value,
                        Postcode = baseNode.Element(_ns2 + "postCode")?.Value,
                        Street = baseNode.Element(_ns2 + "street")?.Value,
                        StreetNumber = baseNode.Element(_ns2 + "streetNumber")?.Value,
                        EvidenceNumber = baseNode.Element(_ns2 + "evidenceNumber")?.Value,
                        DeliveryDetails = baseNode.Element(_ns2 + "deliveryDetails")?.Value,
                        PragueDistrict = baseNode.Element(_ns2 + "pragueDistrict")?.Value,
                        AddressPointId = baseNode.Element(_ns2 + "id")?.Value
                    };
                }
                else
                    return null;
            }
        }
    }

    public async Task<List<Dto.FoundSuggestion>> GetSuggestions(string sessionId, string text, int pageSize, string? country, CancellationToken cancellationToken)
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
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress))
        {
            request.Headers.Add("SOAPAction", "getSuggestions");
            request.Content = content;

            using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                string rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

                response.EnsureSuccessStatusCode();
                var xml = XElement.Parse(rawResponse);

                var responseModel = new List<Dto.FoundSuggestion>();
                foreach (var itm in xml
                    .Descendants(_soapenv + "Body")
                    .Descendants(_ns1 + "getSuggestionsRes")
                    .Descendants(_ns2 + "suggestedAddressList")
                    .Descendants(_ns2 + "suggestedAddress"))
                {
                    responseModel.Add(new Dto.FoundSuggestion
                    {
                        AddressId = itm.Element(_ns2 + "id")!.Value,
                        Title = itm.Element(_ns2 + "title")!.Value
                    });
                }

                if (responseModel.Any())
                    return new List<Dto.FoundSuggestion>(responseModel);
                else
                    return new List<Dto.FoundSuggestion>(0);
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
            <v11:applicationComponent>NOBY.FEAPI</v11:applicationComponent>
         </v11:originator>
         <v11:caller>
            <v11:application>NOBY</v11:application>
            <v11:applicationComponent>NOBY.FEAPI</v11:applicationComponent>
         </v11:caller>
      </v1:systemIdentity>
   </soapenv:Header>";

    private readonly HttpClient _httpClient;
    private readonly CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<IAddressWhispererClient> _configuration;

    public RealAddressWhispererClient(HttpClient httpClient, CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<IAddressWhispererClient> configuration)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }
}
