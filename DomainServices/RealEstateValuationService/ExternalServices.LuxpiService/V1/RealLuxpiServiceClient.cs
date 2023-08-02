using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.RealEstateValuationService.ExternalServices.LuxpiService.V1;

internal sealed class RealLuxpiServiceClient
    : ILuxpiServiceClient
{
    public async Task<Dto.CreateKbmodelFlatResponse> CreateKbmodelFlat(Contracts.KBModelRequest request, long id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + $"/api/KBModel/flat/address/{id}", request, cancellationToken)
            .ConfigureAwait(false);

        var model = await response.EnsureSuccessStatusAndReadJson<Contracts.ValuationRequest>(StartupExtensions.ServiceName, cancellationToken);

        return model.Status switch
        {
            "OK" => createResponse(),
            "KNOCKED_OUT" => throw ErrorCodeMapper.CreateExtServiceValidationException(ErrorCodeMapper.LuxpiKbModelStatusFailed),
            _ => throw ErrorCodeMapper.CreateExtServiceValidationException(ErrorCodeMapper.LuxpiKbModelUnknownStatus, model.Status)
        };

        Dto.CreateKbmodelFlatResponse createResponse()
        {
            if (!model!.ResultPrice.HasValue || !model.ValuationId.HasValue)
            {
                ErrorCodeMapper.CreateExtServiceValidationException(ErrorCodeMapper.LuxpiKbModelIncorrectResult);
            }

            return new Dto.CreateKbmodelFlatResponse
            {
                ResultPrice = Convert.ToInt32(model.ResultPrice!.Value),
                ValuationId = model.ValuationId!.Value
            };
        }
    }

    private readonly HttpClient _httpClient;

    public RealLuxpiServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
