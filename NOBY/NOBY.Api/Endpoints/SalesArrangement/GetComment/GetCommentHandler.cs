using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Api.Endpoints.SalesArrangement.SharedDto;

namespace NOBY.Api.Endpoints.SalesArrangement.GetComment;

internal sealed class GetCommentHandler : IRequestHandler<GetCommentRequest, Comment>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    
    public async Task<Comment> Handle(GetCommentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        
        return salesArrangement.IsProductSalesArrangement()
            ? new Comment { Text = salesArrangement.Mortgage.Comment }
            : throw new NobyValidationException("Sales Arrangement is not of product types");
    }
    
    public GetCommentHandler(ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}