using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Repositories;
using DomainServices.DocumentOnSAService.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.BackgroundServices.CheckDocumentsArchived;

public sealed class CheckDocumentsArchivedJob
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    private const short _successfullyArchivedStatus = 400;

    private readonly CheckDocumentsArchivedJobConfiguration _configuration;
    private readonly IDocumentArchiveServiceClient _documentArchiveServiceClient;
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IBatchOperationRepository _batchOperationRepository;
    private readonly ILogger<CheckDocumentsArchivedJob> _logger;

    public CheckDocumentsArchivedJob(
        CheckDocumentsArchivedJobConfiguration configuration,
        IDocumentArchiveServiceClient documentArchiveServiceClient,
        DocumentOnSAServiceDbContext dbContext,
        IBatchOperationRepository batchOperationRepository,
        ILogger<CheckDocumentsArchivedJob> logger)
    {
        _configuration = configuration;
        _documentArchiveServiceClient = documentArchiveServiceClient;
        _dbContext = dbContext;
        _batchOperationRepository = batchOperationRepository;
        _logger = logger;
    }

    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        var unArchivedEArchiveLinkeds = await _dbContext.EArchivIdsLinked
                                            .AsNoTracking()
                                            .Where(d => d.DocumentOnSa.IsArchived == false)
                                            .Take(_configuration.MaxBatchSize)
                                            .ToListAsync(cancellationToken);

        if (unArchivedEArchiveLinkeds.Count == 0)
            return;

        _logger.UnarchivedDocumentsOnSa(typeof(CheckDocumentsArchivedJob).Name, unArchivedEArchiveLinkeds.Count);

        var successfullyArchivedDocumentIds = await GetSuccessfullyArchivedDocumentIds(unArchivedEArchiveLinkeds.Select(s => s.EArchivId)!, cancellationToken);

        _logger.AlreadyArchived(typeof(CheckDocumentsArchivedJob).Name, unArchivedEArchiveLinkeds.Count, successfullyArchivedDocumentIds.Count);

        if (successfullyArchivedDocumentIds.Count == 0)
            return;


        var docOnSaIdsForUpdate = unArchivedEArchiveLinkeds
                                  .Where(d => successfullyArchivedDocumentIds.Contains(d.EArchivId))
                                  .Select(s => s.DocumentOnSAId)
                                  .Distinct();

        await _batchOperationRepository.UpdateIsDocumentArchiveState(docOnSaIdsForUpdate, cancellationToken);
    }

    private async Task<List<string>> GetSuccessfullyArchivedDocumentIds(IEnumerable<string> unArchivedDocOnSaEaIds, CancellationToken cancellationToken)
    {
        var request = new GetDocumentsInQueueRequest();
        request.EArchivIds.AddRange(unArchivedDocOnSaEaIds);
        var documentInQueue = await _documentArchiveServiceClient.GetDocumentsInQueue(request, cancellationToken);
        return documentInQueue.QueuedDocuments
                                              .Where(d => d.StatusInQueue == _successfullyArchivedStatus)
                                              .Select(s => s.EArchivId)
                                              .ToList();
    }
}
