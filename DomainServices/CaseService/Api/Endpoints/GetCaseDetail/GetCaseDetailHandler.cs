using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.CaseService.Api.Endpoints.GetCaseDetail;

internal class GetCaseDetailHandler
    : IRequestHandler<GetCaseDetailRequest, Case>
{
    /// <summary>
    /// Vraci detail Case-u
    /// </summary>
    public async Task<Case> Handle(GetCaseDetailRequest request, CancellationToken cancellation)
    {
        // vytahnout Case z DB
        return await _dbContext.Cases
            .Where(t => t.CaseId == request.CaseId)
            .AsNoTracking()
            .Select(CaseServiceDatabaseExpressions.CaseDetail())
            .FirstOrDefaultAsync(cancellation) 
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);
    }

    private readonly CaseServiceDbContext _dbContext;

    public GetCaseDetailHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
