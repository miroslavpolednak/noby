namespace DomainServices.RealEstateValuationService.ExternalServices.PreorderService.V1;

internal sealed class MockPreorderServiceClient
    : IPreorderServiceClient
{
    public Task DeleteAttachment(long externalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<long> UploadAttachment(string title, string fileName, string mimeType, byte[] fileData, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
