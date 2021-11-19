namespace CIS.InternalServices.Storage.Abstraction.BlobStorage.Dto;

internal record BlobDeleteRequest(string BlobKey)
    : IRequest
{ }
