using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.DocumentOnSAService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CheckDocumentsArchived;

internal sealed class CheckDocumentsArchivedHandler
    : IJob
{
    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        var configuration = System.Text.Json.JsonSerializer.Deserialize<CheckDocumentsArchivedConfiguration>(jobData ?? "{}");

        var unArchivedEArchiveLinkeds = await _maintananceService.GetCheckDocumentsArchivedIds(configuration?.MaxBatchSize ?? 10, cancellationToken);
        if (unArchivedEArchiveLinkeds.Items.Count == 0)
            return;

        _logger.UnarchivedDocumentsOnSa(unArchivedEArchiveLinkeds.Items.Count);

        var successfullyArchivedDocumentIds = await GetSuccessfullyArchivedDocumentIds(unArchivedEArchiveLinkeds.Items.Select(s => s.EArchivId), cancellationToken);

        _logger.AlreadyArchived(unArchivedEArchiveLinkeds.Items.Count, successfullyArchivedDocumentIds.Count);

        if (successfullyArchivedDocumentIds.Count == 0)
            return;

        var docOnSaIdsForUpdate = unArchivedEArchiveLinkeds
            .Items
            .Where(d => successfullyArchivedDocumentIds.Contains(d.EArchivId))
            .Select(s => s.DocumentOnSAId)
            .Distinct()
            .ToArray();

        await _maintananceService.UpdateDocumentsIsArchived(docOnSaIdsForUpdate, cancellationToken);
    }

    private async Task<List<string>> GetSuccessfullyArchivedDocumentIds(IEnumerable<string> unArchivedDocOnSaEaIds, CancellationToken cancellationToken)
    {
        var request = new GetDocumentsInQueueRequest();
        request.EArchivIds.AddRange(unArchivedDocOnSaEaIds);
        var documentInQueue = await _documentArchiveService.GetDocumentsInQueue(request, cancellationToken);
        return documentInQueue.QueuedDocuments
            .Where(d => d.StatusInQueue == _successfullyArchivedStatus)
            .Select(s => s.EArchivId)
            .ToList();
    }

    private const short _successfullyArchivedStatus = 400;

    private readonly ILogger<CheckDocumentsArchivedHandler> _logger;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IMaintananceService _maintananceService;

    public CheckDocumentsArchivedHandler(IMaintananceService maintananceService, IDocumentOnSAServiceClient documentOnSAService, IDocumentArchiveServiceClient documentArchiveService, ILogger<CheckDocumentsArchivedHandler> logger)
    {
        _maintananceService = maintananceService;
        _documentOnSAService = documentOnSAService;
        _documentArchiveService = documentArchiveService;
        _logger = logger;
    }
}
