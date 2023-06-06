using DomainServices.CaseService.Clients;
using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Api.Database.Entities;
using DomainServices.ProductService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.ProductService.Api.Endpoints.GetCovenantList;

internal sealed class GetCovenantListHandler : IRequestHandler<GetCovenantListRequest, GetCovenantListResponse>
{
    public async Task<GetCovenantListResponse> Handle(GetCovenantListRequest request, CancellationToken cancellationToken)
    {
        await _caseService.ValidateCaseId(request.CaseId, true, cancellationToken);
        
        var covenants = await _dbContext.Covenants
            .Where(c => c.CaseId == request.CaseId)
            .ToListAsync(cancellationToken);

        var covenantPhases = await _dbContext.CovenantPhases
            .Where(c => c.CaseId == request.CaseId)
            .ToListAsync(cancellationToken);
        
        var response = new GetCovenantListResponse();
        response.Covenants.AddRange(covenants.Select(Map));
        response.Phases.AddRange(covenantPhases.Select(Map));
        
        return response;
    }

    private static CovenantListItem Map(Covenant covenant) => new()
    {
        Name = covenant.Name,
        FulfillDate = covenant.FulfillDate,
        IsFulfilled = covenant.IsFulFilled != 0,
        OrderLetter = covenant.OrderLetter,
        PhaseOrder = covenant.PhaseOrder,
        CovenantTypeId = covenant.CovenantTypeId,
    };

    private static PhaseListItem Map(CovenantPhase covenantPhase) => new()
    {
        Name = covenantPhase.Name,
        Order = covenantPhase.Order,
        OrderLetter = covenantPhase.OrderLetter
    };
    
    private readonly ProductServiceDbContext _dbContext;
    private readonly ICaseServiceClient _caseService;
    
    public GetCovenantListHandler(
        ProductServiceDbContext dbContext,
        ICaseServiceClient caseService)
    {
        _dbContext = dbContext;
        _caseService = caseService;
    }
}