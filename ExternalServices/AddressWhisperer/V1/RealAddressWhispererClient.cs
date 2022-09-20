using CIS.Infrastructure.Logging;

namespace ExternalServices.AddressWhisperer.V1;

internal class RealAddressWhispererClient
    : Shared.BaseClient<RealAddressWhispererClient>, IAddressWhispererClient
{
    public async Task<IServiceCallResult> GetAddressDetail(string sessionId, string addressId)
    {
        throw new NotImplementedException();
    }

    public async Task<IServiceCallResult> GetSuggestions(string sessionId, string text, int pageSize, int? countryid)
    {
        return await callMethod(async () =>
        {
            using ServiceClient.AddressWhispererBEServiceClient client = createClient();
            
            var request = new ServiceClient.GetSuggestionsReq
            {
                sessionId = sessionId,
                addressPattern = text,
                paging = new ServiceClient.Paging
                {
                    numberOfEntries = pageSize
                }
            };

            _logger.LogSerializedObject("GetSuggestionsRequest", request);
            var serviceResult = await client.getSuggestionsAsync(request);
            _logger.LogSerializedObject("GetSuggestionsResponse", serviceResult);

            var result = serviceResult?
                .getSuggestionsRes?
                .suggestedAddressList?
                .Select(t => new Shared.FoundSuggestion
                {
                    AddressId = t.id,
                    Title = t.title
                })
                .ToList();

            if (result == null || !result.Any())
                return new EmptyServiceCallResult();
            else
                return new SuccessfulServiceCallResult<List<Shared.FoundSuggestion>>(result);
        });
    }

    public RealAddressWhispererClient(Shared.AddressWhispererConfiguration configuration, ILogger<RealAddressWhispererClient> logger)
        : base(Versions.V1, configuration, logger)
    {
    }

    private ServiceClient.AddressWhispererBEServiceClient createClient()
        => new ServiceClient.AddressWhispererBEServiceClient(createHttpBinding(), createEndpoint());
}
