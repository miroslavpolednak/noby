using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.BackgroundServices.CheckDocumentsArchived;

public sealed class CheckDocumentsArchivedJob
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    private const short SuccessfullyArchivedStatus = 400;

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

        if (!unArchivedEArchiveLinkeds.Any())
            return;

        _logger.LogInformation("{ServiceName}: {Count} unarchived documentsOnSa", typeof(CheckDocumentsArchivedJob).Name, unArchivedEArchiveLinkeds.Count);

        var successfullyArchivedDocumentIds = await GetSuccessfullyArchivedDocumentIds(unArchivedEArchiveLinkeds.Select(s => s.EArchivId)!, cancellationToken);

        _logger.LogInformation("{ServiceName}:From {UnArchCount} unarchived documentsOnSa, {ArchCount} have been already archived}",
                                   typeof(CheckDocumentsArchivedJob).Name,
                                   unArchivedEArchiveLinkeds.Count,
                                   successfullyArchivedDocumentIds.Count);

        if (!successfullyArchivedDocumentIds.Any())
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
        var successfullyArchivedDocumentIds = documentInQueue.QueuedDocuments
                                              .Where(d => d.StatusInQueue == SuccessfullyArchivedStatus)
                                              .Select(s => s.EArchivId)
                                              .ToList();
        return successfullyArchivedDocumentIds;
    }
}
