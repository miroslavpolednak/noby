using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.ESignatures.V1;

namespace CIS.InternalServices.TaskSchedulingService.Api.Jobs.UpdateDocumentStatus;

internal sealed class UpdateDocumentStatusHandler
    : IJob
{
    public async Task Execute(string? jobData, CancellationToken cancellationToken)
    {
        var ids = await _maintananceService.GetUpdateDocumentStatusIds(cancellationToken);

        foreach (var documentOnSa in ids.Items)
        {
            try
            {
                var status = await _eSignaturesClient.GetDocumentStatus(documentOnSa.ExternalIdESignatures!, cancellationToken);

                if (status is EDocumentStatuses.SIGNED or EDocumentStatuses.VERIFIED or EDocumentStatuses.SENT)
                {
                    await _documentOnSAService.SignDocument(documentOnSa.DocumentOnSAId, (int)SignatureTypes.Electronic, cancellationToken);
                }

                if (status == EDocumentStatuses.DELETED)
                {
                    var stopSignRequest = new StopSigningRequest
                    {
                        DocumentOnSAId = documentOnSa.DocumentOnSAId
                    };

                    await _documentOnSAService.StopSigning(stopSignRequest, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.UpdateDocumentStatusFailed(documentOnSa.DocumentOnSAId, ex);
            }
        }
    }

    private readonly IMaintananceService _maintananceService;
    private readonly IESignaturesClient _eSignaturesClient;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly ILogger<UpdateDocumentStatusHandler> _logger;

    public UpdateDocumentStatusHandler(
        IESignaturesClient eSignaturesClient,
        ILogger<UpdateDocumentStatusHandler> logger,
        IMaintananceService maintananceService,
        IDocumentOnSAServiceClient documentOnSAService)
    {
        _eSignaturesClient = eSignaturesClient;
        _logger = logger;
        _maintananceService = maintananceService;
        _documentOnSAService = documentOnSAService;
    }
}
