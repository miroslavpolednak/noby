namespace CIS.InternalServices.Storage.Abstraction.BlobStorageTemp.Dto;

internal record BlobTempGetRequest(string BlobKey)
    : IRequest<Contracts.BlobGetResponse>
{ }
