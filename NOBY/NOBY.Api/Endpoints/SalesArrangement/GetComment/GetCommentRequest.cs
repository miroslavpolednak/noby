using NOBY.Api.Endpoints.SalesArrangement.SharedDto;

namespace NOBY.Api.Endpoints.SalesArrangement.GetComment;

internal sealed class GetCommentRequest : IRequest<Comment>
{
    public int SalesArrangementId { get; }

    public GetCommentRequest(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
    }
}