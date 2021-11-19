namespace CIS.InternalServices.Storage.Abstraction.BlobStorageTemp.Dto;

internal record BlobTempSaveRequest(byte[] Data, string Name = "", string ContentType = "", string? SessionId = null, string? ApplicationKey = null)
    : IRequest<string>
{ }
