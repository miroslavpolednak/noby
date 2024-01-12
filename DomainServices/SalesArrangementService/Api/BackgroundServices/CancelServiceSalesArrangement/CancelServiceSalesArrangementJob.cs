
using CIS.Core;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Api.Database;
using DomainServices.SalesArrangementService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.BackgroundServices.CancelServiceSalesArrangement;

internal sealed class CancelServiceSalesArrangementJob
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    private readonly ICodebookServiceClient _codebookService;
    private readonly SalesArrangementServiceDbContext _dbContext;
    private readonly IDateTime _dateTimeService;
    private readonly IMediator _mediator;
    private readonly ILogger<CancelServiceSalesArrangementJob> _logger;

    public CancelServiceSalesArrangementJob(
        ICodebookServiceClient codebookService,
        SalesArrangementServiceDbContext dbContext,
        IDateTime dateTimeService,
        IMediator mediator,
        ILogger<CancelServiceSalesArrangementJob> logger)
    {
        _codebookService = codebookService;
        _dbContext = dbContext;
        _dateTimeService = dateTimeService;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        // Get all service SalesArrangementTypeId
        var salesArrangementTypes = (await _codebookService.SalesArrangementTypes(cancellationToken)).Where(s => s.SalesArrangementCategory == 2);
        var saIdsForDelete = await _dbContext.SalesArrangements.Where(s =>
        salesArrangementTypes.Select(r => r.Id).Contains(s.SalesArrangementTypeId)
        &&
          (
            (s.FirstSignatureDate == null && s.CreatedTime < _dateTimeService.Now.AddDays(-90))
            ||
            (s.FirstSignatureDate != null && s.FirstSignatureDate < _dateTimeService.Now.AddDays(-40) && s.State != 2) // State = 2 (Předáno ke zpracování)
          )
        )
        .Select(sa => sa.SalesArrangementId)
        .ToListAsync(cancellationToken);

        _logger.DeleteServiceSalesArrangement(saIdsForDelete.Count);

        foreach (var saId in saIdsForDelete)
        {
            await _mediator.Send(new DeleteSalesArrangementRequest { HardDelete = true, SalesArrangementId = saId }, cancellationToken);
        }
    }
}
