using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Api.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.BackgroundServices.CheckDocumentsArchived;

internal sealed class CheckDocumentsArchivedJob
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
        var unArchivedDocOnSaIds = await _dbContext.DocumentOnSa
            .Where(d => !string.IsNullOrEmpty(d.EArchivId) && d.IsDocumentArchived == false)
            .Take(_configuration.MaxBatchSize)
            .Select(s => s.EArchivId)
        .ToListAsync(cancellationToken);

        if (!unArchivedDocOnSaIds.Any())
            return;

        _logger.LogInformation("{ServiceName}: {Count} unarchived documentsOnSa", typeof(CheckDocumentsArchivedJob).Name, unArchivedDocOnSaIds.Count);

        var successfullyArchivedDocumentIds = await GetSuccessfullyArchivedDocumentIds(unArchivedDocOnSaIds!, cancellationToken);

        _logger.LogInformation("{ServiceName}:From {UnArchCount} unarchived documentsOnSa, {ArchCount} have been already archived}",
                                typeof(CheckDocumentsArchivedJob).Name,
                                unArchivedDocOnSaIds.Count,
                                successfullyArchivedDocumentIds.Count);

        if (!successfullyArchivedDocumentIds.Any())
            return;

        await _batchOperationRepository.UpdateIsDocumentArchiveState(successfullyArchivedDocumentIds, cancellationToken);
    }

    private async Task<List<string>> GetSuccessfullyArchivedDocumentIds(List<string> unArchivedDocOnSaIds, CancellationToken cancellationToken)
    {
        var request = new GetDocumentsInQueueRequest();
        request.EArchivIds.AddRange(unArchivedDocOnSaIds);
        var documentInQueue = await _documentArchiveServiceClient.GetDocumentsInQueue(request, cancellationToken);
        var successfullyArchivedDocumentIds = documentInQueue.QueuedDocuments
                                              .Where(d => d.Status == SuccessfullyArchivedStatus)
                                              .Select(s => s.EArchivId)
                                              .ToList();
        return successfullyArchivedDocumentIds;
    }
}
