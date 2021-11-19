using CIS.Core.Types;

namespace CIS.InternalServices.Storage.Api.BlobStorage;

internal interface IBlobStorageProvider
{
    Task Save(byte[] blobData, BlobKey blobKey, BlobKinds kind, ApplicationKey applicationKey);

    Task<byte[]> Get(BlobKey blobKey, BlobKinds kind, ApplicationKey applicationKey);

    Task Delete(BlobKey blobKey, BlobKinds kind, ApplicationKey applicationKey);

    Task Copy(BlobKey blobKey, ApplicationKey applicationKey, BlobKinds kindFrom, BlobKinds kindTo);
}
