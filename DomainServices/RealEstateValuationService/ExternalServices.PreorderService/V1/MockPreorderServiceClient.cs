namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;

internal sealed class MockPreorderServiceClient
    : IPreorderServiceClient
{
    public Task DeleteAttachment(long externalId, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<long> UploadAttachment(string title, string category, string fileName, string mimeType, byte[] fileData, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<long>((new Random()).Next(1, 1000));
    }
}
