using CIS.Core.Exceptions;
using CIS.Core.Extensions;
using CIS.Core.Security;
using CIS.Infrastructure.Logging.Extensions;
using ExternalServices.Sdf.Configuration;
using ExternalServices.Sdf.V1.Model;
using Ixtent.ContentServer.ExtendedServices.Model;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Globalization;
using System.ServiceModel;

namespace ExternalServices.Sdf.V1.Clients
{
    public class SdfClient : ISdfClient, IDisposable
    {
        private const int MaxRetries = 3;

        private readonly ExtendedServicesClient _client;
        private readonly SdfConfiguration _sdfConfiguration;
        private AsyncRetryPolicy _retryPolicy;

        public SdfClient(
            ILogger<SdfClient> logger,
            SdfConfiguration sdfConfiguration,
            ICurrentUserAccessor currentUser)
        {
            _sdfConfiguration = sdfConfiguration;
            _retryPolicy = CreatePolicy();

            var binding = CreateBinding();
            var remoteAddress = new EndpointAddress(sdfConfiguration.ServiceUrl);
            _client = new ExtendedServicesClient(binding, remoteAddress);
            if (sdfConfiguration.EnableSoapMessageLogging)
            {
                _client.Endpoint.SetTraceLogging(logger, SdfConfiguration.SectionName);
            }
        }

        public async Task<GetDocumentByExternalIdOutput> GetDocumentByExternalId(GetDocumentByExternalIdSdfQuery query, CancellationToken cancellation)
        {
            var user = CreateUser();
            var options = CreateOptions(query.WithContent);

            // There is a bug on external service site (EArchiv), even if username and password are correct,
            // external service return exception (invalid username or password) sometimes.
            // Retry logic policy eliminate this problem
            var result = await _retryPolicy.ExecuteAsync(async () =>
                                       await _client.GetDocumentByExternalIdAsync(user, query.DocumentId, options)
                                      .WithCancellation(cancellation));

            return result;

        }

        public void Dispose()
        {
            if (_client.State == CommunicationState.Faulted)
            {
                _client.Abort();
            }
            else
            {
                _client.Close();
            }
        }

        private BasicHttpBinding CreateBinding()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MessageEncoding = WSMessageEncoding.Mtom;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.SendTimeout = TimeSpan.Parse(_sdfConfiguration.Timeout, CultureInfo.InvariantCulture);
            binding.ReceiveTimeout = TimeSpan.Parse(_sdfConfiguration.Timeout, CultureInfo.InvariantCulture);
            return binding;
        }

        private User CreateUser()
        {
            return new User { Username = _sdfConfiguration.Username, Password = _sdfConfiguration.Password };
        }

        private GetDocumentByExternalIdOptions CreateOptions(bool withContent)
        {
            var options = new GetDocumentByExternalIdOptions
            {
                ConfigurationId = "ZiskejDleExternihoID",
                RetrieveAdvancedNodeData = true,
                RetrieveContent = withContent,
                RetrieveDocArcId = false,
                MemberId = "990967y", // _currentUser.UserDetails.DisplayName, 
                MemberIdType = "DmsUserLogin"
            };
            return options;
        }

        private static AsyncRetryPolicy CreatePolicy()
        {
            return Policy.Handle<FaultException>().RetryAsync(MaxRetries, onRetry: (exp, retryCount) =>
            {
                if (exp.Message.Contains("DocumentNotFound"))
                {
                    throw new CisNotFoundException(14003, "Document with ExternalId not found");
                }
            });
        }
    }
}
