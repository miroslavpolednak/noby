using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;

internal sealed class RealPreorderServiceClient
    : IPreorderServiceClient
{
    public async Task<List<CIS.Foms.Enums.RealEstateValuationTypes>> GetValuationTypes(Contracts.AvailableValuationTypesRequestDTO request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + "/lookup/valuationtypes", request, cancellationToken)
            .ConfigureAwait(false);

        var acvResponse = await response.EnsureSuccessStatusAndReadJson<List<string>>(StartupExtensions.ServiceName, cancellationToken);
        return acvResponse.Select(t => getEnum(t.AsSpan())).ToList();

        CIS.Foms.Enums.RealEstateValuationTypes getEnum(ReadOnlySpan<char> s)
            => s switch
            {
                "MODEL" => CIS.Foms.Enums.RealEstateValuationTypes.Online,
                "DTS" => CIS.Foms.Enums.RealEstateValuationTypes.Dts,
                "STANDARD" => CIS.Foms.Enums.RealEstateValuationTypes.Standard,
                _ => throw new CisExtServiceValidationException(0, $"Unknown result '{s}'")
            };
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
            .PostAsync(_httpClient.BaseAddress + "/attachment", content, cancellationToken)
            .ConfigureAwait(false);

        var result = await response.EnsureSuccessStatusAndReadJson<List<Contracts.AttachmentDTO>>(StartupExtensions.ServiceName, cancellationToken);

        if (!result.Any())
        {
            throw ErrorCodeMapper.CreateExtServiceValidationException(ErrorCodeMapper.PreorderSvcUploadAttachmentNoFile);
        }

        return result[0].Id;
    }

    public async Task DeleteAttachment(long externalId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .DeleteAsync(_httpClient.BaseAddress + $"/attachment/{externalId}", cancellationToken)
            .ConfigureAwait(false);

        await response.EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    private readonly HttpClient _httpClient;

    public RealPreorderServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
