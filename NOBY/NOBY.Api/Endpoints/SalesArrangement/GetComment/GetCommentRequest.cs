namespace NOBY.Api.Endpoints.SalesArrangement.GetComment;

internal sealed record GetCommentRequest(int SalesArrangementId)
    : IRequest<SalesArrangementSharedComment>
{
}