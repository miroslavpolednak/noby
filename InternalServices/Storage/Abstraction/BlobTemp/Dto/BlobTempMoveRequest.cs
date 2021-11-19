namespace CIS.InternalServices.Storage.Abstraction.BlobStorageTemp.Dto;

internal record BlobTempMoveRequest(IEnumerable<string> BlobKeys)
    : IRequest
{ }
