using CIS.Foms.Types.Enums;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.SalesArrangement.Dto;

namespace NOBY.Api.Endpoints.SalesArrangement.GetComment;

internal sealed  class GetCommentHandler : IRequestHandler<GetCommentRequest, Comment>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    
    public async Task<Comment> Handle(GetCommentRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        
        return (SalesArrangementTypes)salesArrangement.SalesArrangementTypeId == SalesArrangementTypes.Mortgage
            ? new Comment
            {
                // TODO
                // Text = salesArrangement.Mortgage.Comment
                Text = "Tvrdí, že není politicky exponovaná osoba, ale já myslím, že je."
            }
            : throw new NobyValidationException(90001);
    }
    
    public GetCommentHandler(ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}