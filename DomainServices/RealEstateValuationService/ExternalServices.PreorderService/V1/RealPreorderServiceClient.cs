using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;

internal sealed class RealPreorderServiceClient
    : IPreorderServiceClient
{
    public async Task<long> UploadAttachment(string title, string fileName, string mimeType, byte[] fileData, CancellationToken cancellationToken)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(title), "Description");
        content.Add(new StringContent("xxxxx"), "Category");
        content.Add(new StringContent(DateTime.Now.ToString("s", System.Globalization.CultureInfo.InvariantCulture)), "Date");

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

    private readonly HttpClient _httpClient;

    public RealPreorderServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
