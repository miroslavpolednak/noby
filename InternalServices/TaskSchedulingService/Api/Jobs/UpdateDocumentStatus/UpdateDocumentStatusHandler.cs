using CIS.InternalServices.TaskSchedulingService.Api.Scheduling.Jobs;
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
                    var signRequest = new SignDocumentRequest
                    {
                        DocumentOnSAId = documentOnSa.DocumentOnSAId,
                        SignatureTypeId = (int)SignatureTypes.Electronic
                    };

                    await _mediator.Send(signRequest, cancellationToken);
                }

                if (status == EDocumentStatuses.DELETED)
                {
                    var stopSignRequest = new StopSigningRequest
                    {
                        DocumentOnSAId = documentOnSa.DocumentOnSAId
                    };

                    await _mediator.Send(stopSignRequest, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.UpdateDocumentStatusFailed(documentOnSa.DocumentOnSAId, ex);
            }
        }
    }

    private readonly DomainServices.DocumentOnSAService.Clients.IMaintananceService _maintananceService;
    private readonly IMediator _mediator;
    private readonly IESignaturesClient _eSignaturesClient;
    private readonly ILogger<UpdateDocumentStatusHandler> _logger;

    public UpdateDocumentStatusHandler(
        IMediator mediator,
        IESignaturesClient eSignaturesClient,
        ILogger<UpdateDocumentStatusHandler> logger,
        DomainServices.DocumentOnSAService.Clients.IMaintananceService maintananceService)
    {
        _mediator = mediator;
        _eSignaturesClient = eSignaturesClient;
        _logger = logger;
        _maintananceService = maintananceService;
    }
}
