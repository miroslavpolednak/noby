using NOBY.Api.Endpoints.SalesArrangement.SharedDto;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateComment;

internal sealed class UpdateCommentRequest : IRequest
{
    public int SalesArrangementId { get; }
    
    public Comment Comment { get; }

    public UpdateCommentRequest(int salesArrangementId, Comment comment)
    {
        SalesArrangementId = salesArrangementId;
        Comment = comment;
    }
}