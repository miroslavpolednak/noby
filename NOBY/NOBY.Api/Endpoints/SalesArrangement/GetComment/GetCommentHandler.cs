using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.GetComment;

internal sealed class GetCommentHandler(ISalesArrangementServiceClient _salesArrangementService)
        : IRequestHandler<GetCommentRequest, SalesArrangementSharedComment>
{
    public async Task<SalesArrangementSharedComment> Handle(GetCommentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        
        return salesArrangement.IsProductSalesArrangement()
            ? new SalesArrangementSharedComment { Text = salesArrangement.Mortgage.Comment }
            : throw new NobyValidationException("Sales Arrangement is not of product types");
    }
}