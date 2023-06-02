using DomainServices.CaseService.Clients;
using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Api.Database.Entities;
using DomainServices.ProductService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantDetail;

internal sealed class GetCovenantDetailHandler : IRequestHandler<GetCovenantDetailRequest, GetCovenantDetailResponse>
{
    public async Task<GetCovenantDetailResponse> Handle(GetCovenantDetailRequest request, CancellationToken cancellationToken)
    {
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);
        
        var covenant = await _dbContext.Covenants
            .Where(c => c.CaseId == request.CaseId && c.Order == request.Order)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.NotFound12024);
        
        return new GetCovenantDetailResponse { Covenant = Map(covenant) };
    }

    private static CovenantDetail Map(Covenant covenant) => new()
    {
        Description = covenant.Description,
        Name = covenant.Name,
        Text = covenant.Text,
        FulfillDate = covenant.FulfillDate,
        IsFulfilled = covenant.IsFulFilled != 0

    };
    
    private readonly ProductServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseService;
    
    public GetCovenantDetailHandler(
        ProductServiceDbContext dbContext,
        ICaseServiceClient caseService)
    {
        _dbContext = dbContext;
        _caseService = caseService;
    }
}