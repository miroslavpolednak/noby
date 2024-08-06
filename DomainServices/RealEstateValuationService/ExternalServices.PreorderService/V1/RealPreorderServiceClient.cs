using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RealEstateValuationService.ExternalServices.PreorderService.Dto;
using System.Net;

namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;

internal sealed class RealPreorderServiceClient
    : IPreorderServiceClient
{
    public async Task<List<(int Price, string PriceSourceType)>?> GetOrderResult(long orderId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(getUrl(_httpClient.BaseAddress!, $"order/{orderId}/result"), cancellationToken)
            .ConfigureAwait(false);
        
        var model = await response.EnsureSuccessStatusAndReadJson<Contracts.OrderResultDTO>(StartupExtensions.ServiceName, cancellationToken);
        decimal? futurePrice = (decimal?)model.ResultPrices?.FirstOrDefault(t => t.PriceSourceType == "STANDARD_PRICE_FUTURE")?.Price;
        if (!futurePrice.HasValue)
        {
            futurePrice = (decimal?)model.ResultPrices?.FirstOrDefault(t => t.PriceSourceType == "LAND_PRICE_FUTURE")?.Price;
        }

        List<(int Price, string PriceSourceType)> finalList = new();
        if (model.ResultPrices is not null)
        {
            foreach (var item in model.ResultPrices)
            {
                if (item.Price.HasValue)
                {
                    finalList.Add((Convert.ToInt32(item.Price.Value), item.PriceSourceType ?? ""));
                }
            }
        }

        return finalList;
    }

    public async Task<bool> RevaluationCheck(Contracts.OnlineRevaluationCheckRequestDTO request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsJsonAsync(getUrl(_httpClient.BaseAddress!, "lookup/revaluationcheck"), request, cancellationToken)
            .ConfigureAwait(false);

        var model = await response.EnsureSuccessStatusAndReadJson<Contracts.RevaluationRequiredResponse>(StartupExtensions.ServiceName, cancellationToken);
        return model.RevaluationRequired;
    }

    public async Task<List<SharedTypes.Enums.EnumRealEstateValuationTypes>> GetValuationTypes(Contracts.AvailableValuationTypesRequestDTO request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsJsonAsync(getUrl(_httpClient.BaseAddress!, "lookup/valuationtypes"), request, cancellationToken)
            .ConfigureAwait(false);

        var acvResponse = await response.EnsureSuccessStatusAndReadJson<List<string>>(StartupExtensions.ServiceName, cancellationToken);
        return acvResponse.Select(t => getEnum(t.AsSpan())).ToList();

        SharedTypes.Enums.EnumRealEstateValuationTypes getEnum(ReadOnlySpan<char> s)
            => s switch
            {
                "MODEL" => SharedTypes.Enums.EnumRealEstateValuationTypes.Online,
                "DTS" => SharedTypes.Enums.EnumRealEstateValuationTypes.Dts,
                "STANDARD" => SharedTypes.Enums.EnumRealEstateValuationTypes.Standard,
                _ => throw new CisExternalServiceValidationException(0, $"Unknown result '{s}'")
            };
    }

    public async Task<OrderResponse> CreateOrder(object request, CancellationToken cancellationToken = default)
    {
        var (path, errorCode) = getInfo();
        var response = await _httpClient
            .PostAsJsonAsync(getUrl(_httpClient.BaseAddress!, path), request, cancellationToken)
            .ConfigureAwait(false);

        var acvResponse = await response.EnsureSuccessStatusAndReadJson<Contracts.OrderDTO>(StartupExtensions.ServiceName, new Dictionary<HttpStatusCode, int>
        {
            { HttpStatusCode.BadRequest, errorCode }
        }, cancellationToken);

        return new OrderResponse
        {
            OrderId = acvResponse.StatusDetails.OrderId
        };

        (string Path, int ErrorCode) getInfo()
        {
            return request switch
            {
                Contracts.OnlineMPRequestDTO => ("order/online", ErrorCodeMapper.OrderOnlineBadRequest),
                Contracts.StandardOrderRequestDTO => ("order/standard", ErrorCodeMapper.OrderStandardBadRequest),
                Contracts.DtsFlatRequest => ("order/dts/flat", ErrorCodeMapper.OrderDtsBadRequest),
                Contracts.DtsHouseRequest => ("order/dts/house", ErrorCodeMapper.OrderDtsBadRequest),
                _ => throw new NotImplementedException("Unknown request DTO for Order")
            };
        }
    }

    public async Task<long> UploadAttachment(string title, string category, string fileName, string mimeType, byte[] fileData, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(title), "Description");
        content.Add(new StringContent(category), "Category");

        var contentFile = new ByteArrayContent(fileData);
        contentFile.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(mimeType);
        content.Add(contentFile, "FromFile", fileName);

        var response = await _httpClient
            .PostAsync(getUrl(_httpClient.BaseAddress!, "attachment"), content, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.EnsureSuccessStatusAndReadJson<List<Contracts.AttachmentDTO>>(StartupExtensions.ServiceName, cancellationToken);

        if (result.Count == 0)
        {
            throw ErrorCodeMapper.CreateExternalServiceValidationException(ErrorCodeMapper.PreorderSvcUploadAttachmentNoFile);
        }

        return result[0].Id;
    }

    public async Task DeleteAttachment(long externalId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .DeleteAsync(getUrl(_httpClient.BaseAddress!, $"attachment/{externalId}"), cancellationToken)
            .ConfigureAwait(false);

        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    private static string getUrl(Uri uri, in string path)
    {
        return uri.AbsoluteUri + (uri.AbsoluteUri[^1] == '/' ? "" : "/") + path;
    }

    private readonly HttpClient _httpClient;

    public RealPreorderServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
