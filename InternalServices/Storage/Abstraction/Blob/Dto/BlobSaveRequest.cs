namespace CIS.InternalServices.Storage.Abstraction.BlobStorage.Dto;

internal record BlobSaveRequest(byte[] Data, string Name, string ContentType, string? ApplicationKey)
    : IRequest<string>
{ }
