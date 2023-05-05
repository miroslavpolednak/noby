using NOBY.Api.Endpoints.SalesArrangement.Dto;

namespace NOBY.Api.Endpoints.SalesArrangement.GetComment;

internal sealed class GetCommentRequest : IRequest<Comment>
{
    public int SalesArrangementId { get; }

    public GetCommentRequest(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
    }
}