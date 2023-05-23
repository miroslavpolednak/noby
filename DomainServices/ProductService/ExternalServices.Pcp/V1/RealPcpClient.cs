using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using CIS.Infrastructure.gRPC;
using ExternalServices;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace DomainServices.ProductService.ExternalServices.Pcp.V1;

internal sealed class RealPcpClient 
    : IPcpClient
{
    public async Task<string> CreateProduct(long caseId, long customerKbId, string pcpProductId, CancellationToken cancellationToken = default(CancellationToken))
    {
        string soap = _soapEnvelopeStart + getHeader() + $@"<soapenv:Body>
<v12:createRequest>
    <dto:productInstance>
    <cre:customerInProductInstanceList>
        <cre:customerInProductInstance>
            <dto:kBCustomer>
                <dto:id>{customerKbId}</dto:id>
            </dto:kBCustomer>
            <dto:partyInProductInstanceRole>
                <dto:partyInproductInstanceRoleCode>
                <dto:class>CB_CustomerLoanProductRole</dto:class>
                <dto:code>A</dto:code>
                </dto:partyInproductInstanceRoleCode>
            </dto:partyInProductInstanceRole>
        </cre:customerInProductInstance>
    </cre:customerInProductInstanceList>
    <cre:mktItemInstanceState>
        <dto:class>CB_AgreementState</dto:class>
        <dto:state>PROPOSED</dto:state>
    </cre:mktItemInstanceState>
    <cre:otherMktItemInstanceIdList>
        <cre:otherMktItemInstanceId>
            <dto:class>ID</dto:class>
            <dto:id>{caseId}</dto:id>
        </cre:otherMktItemInstanceId>
    </cre:otherMktItemInstanceIdList>
    <cre:productInOffer>
        <cre:catalogueProductInOffer>
            <cre:catalogueItemId>
                <dto:id>{pcpProductId}</dto:id>
            </cre:catalogueItemId>
        </cre:catalogueProductInOffer>
    </cre:productInOffer>
    <cre:productInstanceInfo>
    </cre:productInstanceInfo>
    </dto:productInstance>
</v12:createRequest>
   </soapenv:Body>" + _soapEnvelopeEnd;

        using (HttpContent content = new StringContent(soap, Encoding.UTF8, "text/xml"))
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress))
        {
            request.Headers.Add("SOAPAction", "");
            request.Content = content;

            using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                string rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

                response.EnsureSuccessStatusCode();

                var matches = _reponseRegex.Match(rawResponse);
                if (matches.Success)
                {
                    return matches.Groups[1].Value;
                }
                else 
                {
                    throw new CisExtServiceValidationException("Response ID not found");
                }
            }
        }
    }

    private static Regex _reponseRegex = new Regex("<NS2:id>(.*?)</NS2:id>", RegexOptions.Compiled);

    private const string _soapEnvelopeStart = @"<soapenv:Envelope xmlns:cre=""http://kb.cz/ProductInstanceBEService/v1/DTO/create"" xmlns:dto=""http://kb.cz/ProductInstanceBEService/v1/DTO"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:v1=""http://kb.cz/DataModel/Technical/Headers/v1"" xmlns:v11=""http://kb.cz/DataModel/Technical/HeaderTypes/v1"" xmlns:v12=""http://kb.cz/ProductInstanceBEService/v1"">";
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
         <v11:traceId>{(Activity.Current?.TraceId.ToHexString() ?? Guid.NewGuid().ToString())[0..15]}</v11:traceId>
         <v11:timestamp>{DateTime.Now:yyyy-MM-ddTHH:mm:ss}+02:00</v11:timestamp>
      </v1:traceContext>
      <v1:systemIdentity>
         <v11:originator>
            <v11:application>NOBY</v11:application>
            <v11:applicationComponent>NOBY.FEAPI</v11:applicationComponent>
         </v11:originator>
         <v11:caller>
            <v11:application>NOBY</v11:application>
            <v11:applicationComponent>NOBY.DS</v11:applicationComponent>
         </v11:caller>
      </v1:systemIdentity>
   </soapenv:Header>";

    private readonly HttpClient _httpClient;
    private readonly IExternalServiceConfiguration<IPcpClient> _configuration;

    public RealPcpClient(HttpClient httpClient, IExternalServiceConfiguration<IPcpClient> configuration)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }
}
