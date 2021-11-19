namespace CIS.InternalServices.Storage.Api.BlobStorage.Dto
{
    internal record GetRequest(
        BlobKey BlobKey, 
        BlobKinds Kind
    )
        : IRequest<Contracts.BlobGetResponse>
    { }
}
