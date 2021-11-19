namespace CIS.InternalServices.Storage.Abstraction.BlobStorage.Dto;

internal record BlobGetRequest(string BlobKey)
    : IRequest<Contracts.BlobGetResponse>
{ }
