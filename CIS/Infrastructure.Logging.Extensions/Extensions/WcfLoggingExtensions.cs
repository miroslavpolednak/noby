using Microsoft.Extensions.Logging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CIS.Infrastructure.Logging.Extensions.Extensions;

public static class WcfLoggingExtensions
{
    public static void SetTraceLogging(this ServiceEndpoint serviceEndpoint, ILogger logger, string url)
    {
        if (logger == null)
            throw new ArgumentNullException(nameof(logger));
        if (logger.IsEnabled(LogLevel.Information))
            serviceEndpoint.EndpointBehaviors.Add(new ClientMessageLoggingBehavior(logger, url));
    }
}

internal sealed class ClientMessageLoggingBehavior :
   IEndpointBehavior
{
    private readonly ILogger _logger;
    private readonly string _url;

    public ClientMessageLoggingBehavior(ILogger logger, string url)
    {
        _logger = logger;
        _url = url;
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        clientRuntime.ClientMessageInspectors.Add(new ClientMessageLogger(_logger, _url));
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
        private readonly string _url;

        public ClientMessageLogger(ILogger logger, string url)
        {
            _logger = logger;
            _url = url;
        }

        public void AfterReceiveReply(ref Message response, object correlationState)
        {
            // copying message to buffer to avoid accidental corruption
            response = Clone(response);

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                  { "Payload", response.ToString() }
            }))
            {
                _logger.SoapPesponsePayload(_url);
            }
        }

        public object? BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // copying message to buffer to avoid accidental corruption
            request = Clone(request);

            var soapAction = request.Headers.Action;
            var soapMethod = soapAction.Split('/').LastOrDefault();

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                  { "Payload", request.ToString() },
            }))
            {
                _logger.SoapRequestPayload(soapMethod ?? string.Empty, _url);
            }

            return null;
        }

        private static Message Clone(Message message)
        {
            return message.CreateBufferedCopy(int.MaxValue).CreateMessage();
        }
    }
}
