using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateComment;

internal sealed  class UpdateCommentHandler(ISalesArrangementServiceClient _salesArrangementService)
        : IRequestHandler<SalesArrangementUpdateCommentRequest>
{
    public async Task Handle(SalesArrangementUpdateCommentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        
        if (!salesArrangement.IsProductSalesArrangement())
        {
            throw new NobyValidationException($"Invalid SalesArrangement id = {request.SalesArrangementId}, must be of type {SalesArrangementCategories.ProductRequest}");
        }

        salesArrangement.Mortgage ??= new SalesArrangementParametersMortgage();
        salesArrangement.Mortgage.Comment = request.Text ?? string.Empty;

        var updateParametersRequest = new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Mortgage = salesArrangement.Mortgage
        };
        
        await _salesArrangementService.UpdateSalesArrangementParameters(updateParametersRequest, cancellationToken);
    }
}