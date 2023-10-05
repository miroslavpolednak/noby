using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace CIS.Infrastructure.ExternalServicesHelpers.Soap;

internal class SoapHttpBasicAuthenticationBehavior
    : IEndpointBehavior
{
    private readonly IExternalServiceConfiguration _configuration;

    public SoapHttpBasicAuthenticationBehavior(IExternalServiceConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        clientRuntime.ClientMessageInspectors.Add(new SoapHttpBasicAuthentication(_configuration));
    }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    {
    }

    public void Validate(ServiceEndpoint endpoint)
    {
    }

    internal sealed class SoapHttpBasicAuthentication 
        : IClientMessageInspector
    {
        private readonly IExternalServiceConfiguration _configuration;

        public SoapHttpBasicAuthentication(IExternalServiceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AfterReceiveReply(ref Message response, object correlationState)
        {
        }

        public object? BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            HttpRequestMessageProperty? httpRequestMessageProperty;
            if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                httpRequestMessageProperty = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                if (httpRequestMessageProperty == null)
                {
                    httpRequestMessageProperty = new HttpRequestMessageProperty();
                    request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessageProperty);
                }
            }
            else
            {
                httpRequestMessageProperty = new HttpRequestMessageProperty();
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessageProperty);
            }

            var header = HttpHandlers.BasicAuthenticationHttpHandler.PrepareAuthorizationHeaderValue(_configuration);
            httpRequestMessageProperty.Headers.Add("Authorization", $"{header.Scheme} {header.Parameter}");

            return null;
        }
    }
}
