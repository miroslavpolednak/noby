using SharedTypes.Enums;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.UpdateComment;

internal sealed  class UpdateCommentHandler : IRequestHandler<UpdateCommentRequest>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public async Task Handle(UpdateCommentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        
        if (!salesArrangement.IsProductSalesArrangement())
        {
            throw new NobyValidationException($"Invalid SalesArrangement id = {request.SalesArrangementId}, must be of type {SalesArrangementCategories.ProductRequest}");
        }

        salesArrangement.Mortgage.Comment = request.Comment.Text ?? string.Empty;

        var updateParametersRequest = new UpdateSalesArrangementParametersRequest
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            Mortgage = salesArrangement.Mortgage
        };
        
        await _salesArrangementService.UpdateSalesArrangementParameters(updateParametersRequest, cancellationToken);
    }
    
    public UpdateCommentHandler(ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}