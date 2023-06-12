using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.SalesArrangement.Dto;

namespace NOBY.Api.Endpoints.SalesArrangement.GetComment;

internal sealed  class GetCommentHandler : IRequestHandler<GetCommentRequest, Comment>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookService;
    
    public async Task<Comment> Handle(GetCommentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var salesArrangementTypes = await _codebookService.SalesArrangementTypes(cancellationToken);
        var salesArrangementType = salesArrangementTypes.Single(t => t.Id == salesArrangement.SalesArrangementTypeId);
        
        return salesArrangementType.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest
            ? new Comment { Text = salesArrangement.Mortgage.Comment }
            : throw new NobyValidationException(90001);
    }
    
    public GetCommentHandler(
        ISalesArrangementServiceClient salesArrangementService,
        ICodebookServiceClient codebookService)
    {
        _salesArrangementService = salesArrangementService;
        _codebookService = codebookService;
    }
}