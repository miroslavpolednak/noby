namespace CIS.InternalServices.Storage.Api.BlobStorage.Dto
{
    internal record DeleteRequest(BlobKey BlobKey)
        : IRequest
    { }
}
