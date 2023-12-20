using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.CreateSalesArrangement;

internal sealed class CreateSalesArrangementRollback
    : IRollbackAction<CreateSalesArrangementRequest>
{
    public async Task ExecuteRollback(Exception exception, CreateSalesArrangementRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RollbackHandlerStarted(nameof(BagKeySalesArrangementId));

        if (_bag.ContainsKey(BagKeySalesArrangementId))
        {
            var id = (int)_bag[BagKeySalesArrangementId]!;

            await _dbContext.SalesArrangementsParameters.Where(t => t.SalesArrangementId == id).ExecuteDeleteAsync(cancellationToken);
            await _dbContext.SalesArrangements.Where(t => t.SalesArrangementId == id).ExecuteDeleteAsync(cancellationToken);

            _logger.RollbackHandlerStepDone(BagKeySalesArrangementId, _bag[BagKeySalesArrangementId]!);
        }
    }

    public const string BagKeySalesArrangementId = "SAId";

    private readonly IRollbackBag _bag;
    private readonly ILogger<CreateSalesArrangementRollback> _logger;
    private readonly Database.SalesArrangementServiceDbContext _dbContext;

    public CreateSalesArrangementRollback(IRollbackBag bag, ILogger<CreateSalesArrangementRollback> logger, Database.SalesArrangementServiceDbContext dbContext)
    {
        _bag = bag;
        _logger = logger;
        _dbContext = dbContext;
    }
}
