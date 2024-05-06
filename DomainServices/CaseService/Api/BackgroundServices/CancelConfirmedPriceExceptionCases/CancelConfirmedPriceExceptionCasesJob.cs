using DomainServices.CaseService.Contracts;
using Quartz;

namespace DomainServices.CaseService.Api.BackgroundServices.CancelConfirmedPriceExceptionCases;

internal sealed class CancelConfirmedPriceExceptionCasesJob
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        var d = DateOnly.FromDateTime(_timeProvider.GetLocalNow().Date.AddDays(-45));

        var list = await _dbContext
            .ConfirmedPriceExceptions
            .AsNoTracking()
            .Where(t => t.ConfirmedDate < d)
            .Select(t => t.CaseId)
            .ToListAsync(cancellationToken);

        foreach (long caseId in list)
        {
            try
            {
                await _mediator.Send(new CancelCaseRequest { CaseId = caseId }, cancellationToken);

                await _dbContext
                    .ConfirmedPriceExceptions
                    .Where(t => t.CaseId == caseId)
                    .ExecuteDeleteAsync(cancellationToken);
            }
            catch
            {
            }
        }
    }

    private readonly TimeProvider _timeProvider;
    private readonly Database.CaseServiceDbContext _dbContext;
    private readonly IMediator _mediator;

    public CancelConfirmedPriceExceptionCasesJob(TimeProvider timeProvider, Database.CaseServiceDbContext dbContext, IMediator mediator)
    {
        _timeProvider = timeProvider;
        _dbContext = dbContext;
        _mediator = mediator;
    }
}

