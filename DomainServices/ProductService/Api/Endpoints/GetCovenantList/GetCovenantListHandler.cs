using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantList;

internal sealed class GetCovenantListHandler : IRequestHandler<GetCovenantListRequest, GetCovenantListResponse>
{
    public async Task<GetCovenantListResponse> Handle(GetCovenantListRequest request, CancellationToken cancellationToken)
    {
        var covenants = await _dbContext.Covenants
            .Where(c => c.CaseId == request.CaseId)
            .ToListAsync(cancellationToken);

        var covenantPhases = await _dbContext.CovenantPhases
            .Where(c => c.CaseId == request.CaseId)
            .ToListAsync(cancellationToken);

        var result = covenants
            .GroupJoin(
                covenantPhases,
                cov => new { C = cov.CaseId, O = cov.PhaseOrder },
                phase => new { C = phase.CaseId, O = phase.Order},
                (c, cp) =>
                {
                    var item = new CovenantListItem
                    {
                        Name = c.Name,
                        FulfillDate = c.FulfillDate,
                        IsFulfilled = c.IsFulFilled != 0,
                        OrderLetter = c.OrderLetter,
                        PhaseOrder = c.PhaseOrder,
                        CovenantTypeId = c.CovenantTypeId,
                    };

                    item.Phases.AddRange( cp
                        .Select(p => new Phase
                        {
                            Name = p.Name,
                            Order = p.Order,
                            OrderLetter = p.OrderLetter
                        })
                        .ToList());
                    
                    return item;
                })
            .ToList();
        
        
        var response = new GetCovenantListResponse();
        response.Covenants.AddRange(result);
        return response;
    }

    private readonly ProductServiceDbContext _dbContext;
    
    public GetCovenantListHandler(ProductServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}