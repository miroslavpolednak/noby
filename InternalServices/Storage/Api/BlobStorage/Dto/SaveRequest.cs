namespace CIS.InternalServices.Storage.Api.BlobStorage.Dto;

internal record SaveRequest(Core.Types.ApplicationKey ApplicationKey, string Name, string ContentType, BlobKinds Kind, byte[] Data, SessionId? SessionId = null)
    : IRequest<BlobKey>
{
}
