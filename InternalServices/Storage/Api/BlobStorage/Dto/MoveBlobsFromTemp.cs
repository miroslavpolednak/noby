namespace CIS.InternalServices.Storage.Api.BlobStorage.Dto
{
    internal record MoveBlobsFromTemp(List<BlobKey> BlobKey)
        : IRequest
    { }
}
