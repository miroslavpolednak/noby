using CIS.Core.Exceptions;
using CIS.Core.Extensions;
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

namespace ExternalServices.Sdf.V1.Clients;

public class SdfClient : ISdfClient, IDisposable
{
    private const int MaxRetries = 3;

    private readonly ExtendedServicesClient _client;
    private readonly SdfConfiguration _sdfConfiguration;
    private AsyncRetryPolicy _retryPolicy;
    private ILogger<SdfClient> _logger;
    public SdfClient(
        ILogger<SdfClient> logger,
        SdfConfiguration sdfConfiguration
        )
    {
        _sdfConfiguration = sdfConfiguration;
        _logger = logger;
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
        var options = CreateOptionsDocByExternalId(query.WithContent, query.UserLogin);

        // There is a bug on external service site (EArchiv), even if username and password are correct,
        // external service return exception (invalid username or password) sometimes.
        // Retry logic policy eliminate this problem
        var result = await _retryPolicy.ExecuteAsync(async () =>
                                   await _client.GetDocumentByExternalIdAsync(user, query.DocumentId, options)
                                  .WithCancellation(cancellation));

        return result;
    }

    public async Task<FindDocumentsOutput> FindDocuments(FindSdfDocumentsQuery query, CancellationToken cancellationToken)
    {
        var user = CreateUser();

        var options = new FindDocumentsOptions
        {
            ConfigurationId = "ImplicitniDotaz",
            RetrieveDocArcId = false,
            RetrieveAdvancedNodeData = true,
            MemberIdType = "DmsUserLogin",
            MemberId = query.UserLogin
        };

        SearchQueryOptions searchQuery = CreateQueryParameters(query);

        var result = await _retryPolicy.ExecuteAsync(async () =>
                                 await _client.FindDocumentsAsync(user, searchQuery, options)
                                .WithCancellation(cancellationToken));

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
        binding.MaxBufferSize = 2147483647;
        binding.MaxReceivedMessageSize = 2147483647;
        binding.SendTimeout = TimeSpan.Parse(_sdfConfiguration.Timeout, CultureInfo.InvariantCulture);
        binding.ReceiveTimeout = TimeSpan.Parse(_sdfConfiguration.Timeout, CultureInfo.InvariantCulture);
        return binding;
    }

    private User CreateUser()
    {
        return new User { Username = _sdfConfiguration.Username, Password = _sdfConfiguration.Password };
    }

    private GetDocumentByExternalIdOptions CreateOptionsDocByExternalId(bool withContent, string userLogin)
    {
        var options = new GetDocumentByExternalIdOptions
        {
            ConfigurationId = "ZiskejDleExternihoID",
            RetrieveAdvancedNodeData = true,
            RetrieveContent = withContent,
            RetrieveDocArcId = false,
            MemberId = userLogin,
            MemberIdType = "DmsUserLogin"
        };
        return options;
    }

    private AsyncRetryPolicy CreatePolicy()
    {
        return Policy.Handle<FaultException>().RetryAsync(MaxRetries, onRetry: (exp, retryCount) =>
        {
            if (exp.Message.Contains("DocumentNotFound"))
            {
                throw new CisNotFoundException(14003, "Document with ExternalId not found");
            }
            else if (exp.Message.Contains("Invalid username/password specified"))
            {
                _logger.LogInformation("Sdf reported: Invalid username/password specified");
            }
        });
    }

    private static SearchQueryOptions CreateQueryParameters(FindSdfDocumentsQuery query)
    {
        var queryParameters = new List<QueryParameter>();
        if (query.CaseId is not null)
        {
            queryParameters.Add(new QueryParameter
            {
                Attribute = "OP_Cislo_pripadu",
                InputGroup = "Obchodni_pripad",
                Value = query.CaseId.ToString()
            });
        }

        if (!string.IsNullOrWhiteSpace(query.AuthorUserLogin))
        {
            queryParameters.Add(new QueryParameter
            {
                Attribute = "DOK_Autor",
                InputGroup = "Dokument",
                Value = query.AuthorUserLogin
            });
        }

        if (query.CreatedOn is not null)
        {

            queryParameters.Add(new QueryParameter
            {
                Attribute = "DOK_Datum_prijeti",
                InputGroup = "Dokument",
                //Format 2022-08-02
                Value = query.CreatedOn.Value.ToString("yyyy-MM-dd")
            });
        }

        if (!string.IsNullOrWhiteSpace(query.PledgeAgreementNumber))
        {
            queryParameters.Add(new QueryParameter
            {
                Attribute = "DOK_Cislo_zastavni_smlouvy",
                InputGroup = "Dokument",
                Value = query.PledgeAgreementNumber
            });
        }

        if (!string.IsNullOrWhiteSpace(query.ContractNumber))
        {
            queryParameters.Add(new QueryParameter
            {
                Attribute = "OP_Cislo_smlouvy",
                InputGroup = "Obchodni_pripad",
                Value = query.ContractNumber
            });
        }

        if (query.OrderId is not null)
        {
            queryParameters.Add(new QueryParameter
            {
                Attribute = "DOK_ID_oceneni",
                InputGroup = "Dokument",
                Value = query.OrderId.Value.ToString()
            });
        }

        if (!string.IsNullOrWhiteSpace(query.FolderDocumentId))
        {
            queryParameters.Add(new QueryParameter
            {
                Attribute = "DOK_Vazba_pro_SP",
                InputGroup = "Obchodni_pripad",
                Value = query.FolderDocumentId
            });
        }

        var searchQuery = new SearchQueryOptions();
        searchQuery.Parameters = queryParameters.ToArray();
        return searchQuery;
    }
}
