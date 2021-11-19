namespace CIS.InternalServices.Storage.Api.BlobStorage.Dto
{
    internal record MoveSessionFromTemp(SessionId SessionId)
        : IRequest
    { }
}
