using NOBY.Api.Endpoints.SalesArrangement.Dto;

namespace NOBY.Api.Endpoints.SalesArrangement.GetComment;

internal sealed  class GetCommentHandler : IRequestHandler<GetCommentRequest, Comment>
{
    public Task<Comment> Handle(GetCommentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}