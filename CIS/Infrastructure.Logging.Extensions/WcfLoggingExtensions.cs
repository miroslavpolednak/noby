using Microsoft.Extensions.Logging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CIS.Infrastructure.Logging.Extensions
{
    public static class WcfLoggingExtensions
    {
        public static void SetTraceLogging(this ServiceEndpoint serviceEndpoint, ILogger logger, string logMask)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (logger.IsEnabled(LogLevel.Information))
                serviceEndpoint.EndpointBehaviors.Add(new ClientMessageLoggingBehavior(logger, logMask));
        }
    }

    internal sealed class ClientMessageLoggingBehavior :
       IEndpointBehavior
    {
        private readonly ILogger _logger;
        private readonly string _logMask;

        public ClientMessageLoggingBehavior(ILogger logger, string logMask)
        {
            _logger = logger;
            _logMask = logMask;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new ClientMessageLogger(_logger, _logMask));
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
            private readonly string _logMask;

            public ClientMessageLogger(ILogger logger, string logMask)
            {
                _logger = logger;
                _logMask = logMask;
            }

            public void AfterReceiveReply(ref Message response, object correlationState)
            {
                // copying message to buffer to avoid accidental corruption
                response = Clone(response);
                _logger.LogInformation("{LogMask}: Received SOAP response:\r\n{Response}", _logMask, response.ToString());

            }

            public object? BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                // copying message to buffer to avoid accidental corruption
                request = Clone(request);
                _logger.LogInformation("{LogMask}: Sending SOAP request:\r\n{0}", _logMask, request.ToString());
                return null;
            }

            private static Message Clone(Message message)
            {
                return message.CreateBufferedCopy(int.MaxValue).CreateMessage();
            }
        }
    }
}
