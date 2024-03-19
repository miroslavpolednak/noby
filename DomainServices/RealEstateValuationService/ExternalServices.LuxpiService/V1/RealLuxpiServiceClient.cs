using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.Logging;

namespace DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1;

internal sealed class RealLuxpiServiceClient
    : ILuxpiServiceClient
{
    public async Task<Dto.CreateKbmodelFlatResponse> CreateKbmodelFlat(Contracts.KBModelRequest request, long id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsJsonAsync(getUrl(), request, cancellationToken)
            .ConfigureAwait(false);
        
        var model = await response.EnsureSuccessStatusAndReadJson<Contracts.ValuationRequest>(StartupExtensions.ServiceName, cancellationToken);
        
        return model.Status switch
        {
            "OK" => createResponse(),
            "KNOCKED_OUT" or "NO_PRICE_AVAILABLE" => new Dto.CreateKbmodelFlatResponse
            {
                NoPriceAvailable = true
            },
            _ => throw ErrorCodeMapper.CreateExternalServiceValidationException(ErrorCodeMapper.LuxpiKbModelUnknownStatus, model.Status)
        };

        string getUrl()
        {
            return _httpClient.BaseAddress!.AbsoluteUri + (_httpClient.BaseAddress!.AbsoluteUri[^1] == '/' ? "" : "/") + $"api/KBModel/flat/address/{id}";
        }

        Dto.CreateKbmodelFlatResponse createResponse()
        {
            if (!model!.ResultPrice.HasValue || model.Id == 0)
            {
                throw ErrorCodeMapper.CreateExternalServiceValidationException(ErrorCodeMapper.LuxpiKbModelIncorrectResult);
            }
        
            return new Dto.CreateKbmodelFlatResponse
            {
                ResultPrice = Convert.ToInt32(model.ResultPrice!.Value),
                ValuationId = model.Id
            };
        }
    }

    private readonly ILogger<RealLuxpiServiceClient> _logger;
    private readonly HttpClient _httpClient;

    public RealLuxpiServiceClient(HttpClient httpClient, ILogger<RealLuxpiServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
}
