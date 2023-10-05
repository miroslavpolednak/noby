using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace CIS.Infrastructure.ExternalServicesHelpers;

internal sealed class SoapClientMessageLoggingBehavior 
    : IEndpointBehavior
{
    private readonly ILogger _logger;
    private readonly IExternalServiceConfiguration _configuration;

    public SoapClientMessageLoggingBehavior(ILogger logger, IExternalServiceConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        clientRuntime.ClientMessageInspectors.Add(new ClientMessageLogger(_logger, _configuration));
    }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    {
    }

    public void Validate(ServiceEndpoint endpoint)
    {
    }

    internal sealed class ClientMessageLogger :
        IClientMessageInspector
    {
        private readonly ILogger _logger;
        private readonly IExternalServiceConfiguration _configuration;

        public ClientMessageLogger(ILogger logger, IExternalServiceConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void AfterReceiveReply(ref Message response, object correlationState)
        {
            var messageContent = "";
            if (_configuration.LogResponsePayload)
            {
                // copying message to buffer to avoid accidental corruption
                var buffer = response.CreateBufferedCopy(int.MaxValue);
                var message = buffer.CreateMessage();

                //Assign a copy to the ref response (this response isn't readed, so response can be processed)
                response = buffer.CreateMessage();
                messageContent = ReadSoapMessageContent(message);
            }
            
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                  { "Payload", messageContent }
            }))
            {
                _logger.SoapResponsePayload(_configuration.ServiceUrl!.AbsoluteUri);
            }
        }

        public object? BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var messageContent = "";

            if (_configuration.LogRequestPayload)
            {
                // copying message to buffer to avoid accidental corruption
                var buffer = request.CreateBufferedCopy(int.MaxValue);
                var message = buffer.CreateMessage();

                //Assign a copy to the ref request (this request isn't readed, so request can be processed)
                request = buffer.CreateMessage();
                messageContent = ReadSoapMessageContent(message);
            }

            var soapAction = request.Headers.Action;
            var soapMethod = soapAction.Split('/').LastOrDefault();

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "Payload", messageContent }
            }))
            {
                _logger.SoapRequestPayload(soapMethod ?? string.Empty, _configuration.ServiceUrl!.AbsoluteUri);
            }

            return null;
        }

        private static string ReadSoapMessageContent(Message message)
        {
            var sb = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true }))
            {
                message.WriteMessage(xmlWriter);
                xmlWriter.Close();
            }

            return sb.ToString();
        }
    }
}
