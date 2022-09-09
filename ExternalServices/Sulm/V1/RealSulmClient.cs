using System.Text;

namespace ExternalServices.Sulm.V1;

internal sealed class RealSulmClient 
    : ISulmClient
{
    public async Task<IServiceCallResult> StartUse(long partyId, string usageCode)
    {
        string text = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://correlation.kb.cz/datatypes/1/0"" xmlns:ns1=""http://esb.kb.cz/core/dataTypes/1/0"" xmlns:ns2=""http://esb.kb.cz/Sulm/interface/1/0"">
   <soapenv:Header/>
   <soapenv:Body>
      <ns2:startUseRequest>
         <partyId>{partyId}</partyId>
         <usageCode>{usageCode}</usageCode>
      </ns2:startUseRequest>
   </soapenv:Body>
</soapenv:Envelope>";

        using (HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml"))
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _configuration.ServiceUrl))
        {
            request.Headers.Add("SOAPAction", "");
            request.Content = content;
            using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> StopUse(long partyId, string usageCode) 
    {
        string text = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://correlation.kb.cz/datatypes/1/0"" xmlns:ns1=""http://esb.kb.cz/core/dataTypes/1/0"" xmlns:ns2=""http://esb.kb.cz/Sulm/interface/1/0"">
   <soapenv:Header/>
   <soapenv:Body>
      <ns2:stopUseRequest>
         <partyId>{partyId}</partyId>
         <usageCode>{usageCode}</usageCode>
      </ns2:stopUseRequest>
   </soapenv:Body>
</soapenv:Envelope>";

        using (HttpContent content = new StringContent(text, Encoding.UTF8, "text/xml"))
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _configuration.ServiceUrl))
        {
            request.Headers.Add("SOAPAction", "");
            request.Content = content;
            using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        return new SuccessfulServiceCallResult();
    }

    private readonly HttpClient _httpClient;
    private readonly SulmConfiguration _configuration;

    public RealSulmClient(HttpClient httpClient, SulmConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }
}
