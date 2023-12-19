using Microsoft.Extensions.Logging;
using System.ServiceModel.Channels;
using System.ServiceModel;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using Ixtent.ContentServer.ExtendedServices.Model.WebService;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Model;
using Ixtent.ContentServer.ExtendedServices.Model;
using CIS.Core.Extensions;
using Polly;
using CIS.Infrastructure.ExternalServicesHelpers.Soap;
using System.Globalization;

namespace DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Clients;
internal class RealSdfClient : SoapClientBase<ExtendedServicesClient, IExtendedServices>, ISdfClient
{
    private const int _maxRetries = 3;

    private readonly ResiliencePipeline _retryPolicy;
    private readonly ILogger<RealSdfClient> _logger;

    protected override string ServiceName => StartupExtensions.ServiceName;

    public RealSdfClient(
            ILogger<RealSdfClient> logger,
            IExternalServiceConfiguration<ISdfClient> configuration)
            : base(configuration, logger)
    {
        _logger = logger;
        _retryPolicy = CreatePolicy();
    }

    public async Task<GetDocumentByExternalIdOutput> GetDocumentByExternalId(GetDocumentByExternalIdSdfQuery query, CancellationToken cancellation)
    {
        var user = CreateUser();
        var options = CreateOptionsDocByExternalId(query.WithContent, query.UserLogin);

        // There is a bug on external service site (EArchiv), even if username and password are correct,
        // external service return exception (invalid username or password) sometimes.
        // Retry logic policy eliminate this problem
        var result = await _retryPolicy.ExecuteAsync(async (cancellation) =>
                                   await Client.GetDocumentByExternalIdAsync(user, query.DocumentId, options)
                                  .WithCancellation(cancellation), cancellation);

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

        var result = await _retryPolicy.ExecuteAsync(async (cancellationToken) =>
                                 await Client.FindDocumentsAsync(user, searchQuery, options)
                                .WithCancellation(cancellationToken), cancellationToken);

        return result;
    }

    protected override Binding CreateBinding()
    {
        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        binding.MessageEncoding = WSMessageEncoding.Mtom;
        binding.MaxBufferSize = 2147483647;
        binding.MaxReceivedMessageSize = 2147483647;
        binding.SendTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout!.Value!);
        binding.ReceiveTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout!.Value!);
        return binding;
    }

    private User CreateUser()
    {
        return new User { Username = Configuration.Username, Password = Configuration.Password };
    }

    private static GetDocumentByExternalIdOptions CreateOptionsDocByExternalId(bool withContent, string userLogin)
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

    private static ResiliencePipeline CreatePolicy()
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(new Polly.Retry.RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<FaultException>(),
                MaxRetryAttempts = _maxRetries,
                OnRetry = static res =>
                {
                    if ((res.Outcome.Exception?.Message ?? "").Contains("DocumentNotFound"))
                    {
                        throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CspDocumentNotFound);
                    }
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
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
                Value = query.CaseId.Value.ToString(CultureInfo.InvariantCulture)
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
                Value = query.CreatedOn.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
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
                Value = query.OrderId.Value.ToString(CultureInfo.InvariantCulture)
            });
        }

        if (!string.IsNullOrWhiteSpace(query.FolderDocumentId))
        {
            queryParameters.Add(new QueryParameter
            {
                Attribute = "DOK_Vazba_pro_SP",
                InputGroup = "Dokument",
                Value = query.FolderDocumentId
            });
        }

        if (!string.IsNullOrWhiteSpace(query.FormId))
        {
            queryParameters.Add(new QueryParameter
            {
                Attribute = "DOK_ID_formulare",
                InputGroup = "Dokument",
                Value = query.FormId
            });
        }

        var searchQuery = new SearchQueryOptions();
        searchQuery.Parameters = queryParameters.ToArray();
        return searchQuery;
    }
}
