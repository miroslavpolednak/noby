using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.DocumentOnSAService.Clients;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.CheckDocumentsArchived;

internal sealed class CheckDocumentsArchivedHandler
    : IJob
{
    public Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IMaintananceService _maintananceService;

    public CheckDocumentsArchivedHandler(IMaintananceService maintananceService, IDocumentOnSAServiceClient documentOnSAService)
    {
        _maintananceService = maintananceService;
        _documentOnSAService = documentOnSAService;
    }
}
