namespace CIS.InternalServices.Storage.Abstraction.BlobStorageTemp.Dto;

internal record BlobTempMoveSessionRequest(string SessionId)
    : IRequest
{ }
