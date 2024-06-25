using CIS.Core.Extensions;
using CIS.Infrastructure.ExternalServicesHelpers.Configuration;
using CIS.Infrastructure.ExternalServicesHelpers.Soap;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ExternalServices.Party.V1.Clients;

internal class RealPartyClient : SoapClientBase<PartyGeneralBEServiceClient, PartyGeneralBEService>, IPartyClient
{
    private const int _numberOfSuggestion = 50;

    protected override string ServiceName => StartupExtensions.ServiceName;

    public RealPartyClient(ILogger<RealPartyClient> logger,
            IExternalServiceConfiguration<IPartyClient> configuration)
            : base(configuration, logger)
    {
    }

    public async Task<SuggestJuridicalPersonsResponse1> SuggestJuridicalPersons(string countryCode, string searchText, string v33UserId, CancellationToken cancellationToken)
    {
        return await callMethod(async () =>
        {
            var systemIdentity = CreateSystemIdentity();
            var userIdentity = CreateUserIdentity(v33UserId);
            var traceContext = CreateTraceContext();
            var suggestJuridicalPersonsRequest = CreateSuggestJuridicalPersonsRequest(countryCode, searchText);

            using (new OperationContextScope(Client.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageHeaders.Add(
                     new WsseSoapSecurityHeader(Configuration.Username!, Configuration.Password!, NonceGenerator.GetNonce(), DateTime.Now));

                return await Client.suggestJuridicalPersonsAsync(systemIdentity, userIdentity, traceContext, suggestJuridicalPersonsRequest).WithCancellation(cancellationToken);
            }
        });
    }

    public async Task<GetRESInfoResponse1> GetRESInfo(string countryCode, string cin, string v33UserId, CancellationToken cancellationToken)
    {
        return await callMethod(async () =>
        {
            var systemIdentity = CreateSystemIdentity();
            var userIdentity = CreateUserIdentity(v33UserId);
            var traceContext = CreateTraceContext();
            var restInfoRequest = CreateRestInfoRequest(countryCode, cin);

            using (new OperationContextScope(Client.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageHeaders.Add(
                     new WsseSoapSecurityHeader(Configuration.Username!, Configuration.Password!, NonceGenerator.GetNonce(), DateTime.Now));

                return await Client.getRESInfoAsync(systemIdentity, userIdentity, traceContext, restInfoRequest).WithCancellation(cancellationToken);
            }
        });
    }

    protected override Binding CreateBinding()
    {
        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        binding.MessageEncoding = WSMessageEncoding.Mtom;
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
        binding.MaxBufferSize = 2147483647;
        binding.MaxReceivedMessageSize = 2147483647;
        binding.UseDefaultWebProxy = false;
        binding.SendTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout!.Value!);
        binding.ReceiveTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout!.Value!);
        return binding;
    }

    private static SuggestJuridicalPersonsRequest CreateSuggestJuridicalPersonsRequest(string countryCode, string searchText)
    {
        return new SuggestJuridicalPersonsRequest
        {
            juridicalPerson = new LegalPerson_Req { registrationCountry = countryCode },
            paging = new Paging { numberOfEntries = _numberOfSuggestion },
            pattern = new Pattern
            {
                juridicalPersonPattern = new JuridicalPersonPattern
                {
                    name = searchText
                }
            }
        };
    }

    private static GetRESInfoRequest CreateRestInfoRequest(string countryCode, string cin)
    {
        return new GetRESInfoRequest
        {
            juridicalPerson = new JuridicalPersonGRIReq
            {
                orgIdentification = cin,
                registrationCountry = countryCode
            }
        };
    }

    private static UserIdentity CreateUserIdentity(string v33UserId) => new UserIdentity
    {
        Items = [new HumanUser {
               Item = v33UserId,
               ItemElementName = ItemChoiceType.id
           }]
    };

    private static SystemIdentity CreateSystemIdentity()
    {
        return new SystemIdentity
        {
            caller = new Application
            {
                application = "NOBY",
                applicationComponent = "NOBY.DS"
            },
            originator = new Application
            {
                application = "NOBY",
                applicationComponent = "NOBY.FEAPI"
            }
        };
    }

    private static TraceContext CreateTraceContext()
    {
        return new TraceContext
        {
            traceId = (Activity.Current?.TraceId.ToHexString() ?? Guid.NewGuid().ToString())[0..15],
            timestamp = DateTime.Now,
            timestampSpecified = true
        };
    }
}
