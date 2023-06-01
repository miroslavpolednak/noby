using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantDetail;

internal sealed class GetCovenantDetailHandler : IRequestHandler<GetCovenantDetailRequest, GetCovenantDetailResponse>
{
    public async Task<GetCovenantDetailResponse> Handle(GetCovenantDetailRequest request, CancellationToken cancellationToken)
    {
        var covenant = await _dbContext.Covenants
            .Where(c => c.CaseId == request.CaseId && c.Order == request.Order)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new CisNotFoundException(0, "TODO");
        
        return new GetCovenantDetailResponse
        {
            Covenant = new CovenantDetail
            {
                Description = covenant.Description,
                Name = covenant.Name,
                Text = covenant.Text,
                FulfillDate = covenant.FulfillDate,
                IsFulfilled = covenant.IsFulFilled != 0
            }
        };
    }

    private readonly ProductServiceDbContext _dbContext;
    
    public GetCovenantDetailHandler(ProductServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}