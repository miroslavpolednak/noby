using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.CaseService.Api.Database;
using DomainServices.CaseService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.CaseService.Api.Endpoints.v1.CreateCase;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.AsImplementedInterfacesService]
internal sealed class CreateCaseRollback
    : IRollbackAction<CreateCaseRequest>
{
    public async Task ExecuteRollback(Exception exception, CreateCaseRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RollbackHandlerStarted(nameof(CreateCaseRollback));

        // smazat domacnost a customery
        if (_bag.ContainsKey(BagKeyCaseId))
        {
            await _dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM dbo.[Case] WHERE CaseId={(long)_bag[BagKeyCaseId]!}", cancellationToken);
            _logger.RollbackHandlerStepDone(BagKeyCaseId, _bag[BagKeyCaseId]!);
        }
    }

    public const string BagKeyCaseId = "CaseId";

    private readonly IRollbackBag _bag;
    private readonly ILogger<CreateCaseRollback> _logger;
    private readonly CaseServiceDbContext _dbContext;

    public CreateCaseRollback(
        IRollbackBag bag,
        ILogger<CreateCaseRollback> logger,
        CaseServiceDbContext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _bag = bag;
    }
}
