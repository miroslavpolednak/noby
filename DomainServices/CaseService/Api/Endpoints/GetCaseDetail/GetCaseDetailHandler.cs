using CIS.Foms.Enums;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Endpoints.GetCaseDetail;

internal sealed class GetCaseDetailHandler
    : IRequestHandler<GetCaseDetailRequest, Case>
{
    /// <summary>
    /// Vraci detail Case-u
    /// </summary>
    public async Task<Case> Handle(GetCaseDetailRequest request, CancellationToken cancellation)
    {
        // vytahnout Case z DB
        var model = await _dbContext.Cases
            .Where(t => t.CaseId == request.CaseId)
            .AsNoTracking()
            .Select(CaseServiceDatabaseExpressions.CaseDetail())
            .FirstOrDefaultAsync(cancellation) 
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        if (_disallowedStates.Contains(model.State))
        {
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CaseCancelled);
        }

        return model;
    }

    private static int[] _disallowedStates = new[]
    {
        (int)CaseStates.ToBeCancelledConfirmed
    };

    private readonly CaseServiceDbContext _dbContext;

    public GetCaseDetailHandler(CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
