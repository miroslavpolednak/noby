using CIS.Foms.Enums;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.ESignatures.V1;
using FastEnumUtility;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.BackgroundServices.UpdateDocumentStatus;

public sealed class UpdateDocumentStatusJob
    : CIS.Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    private readonly IMediator _mediator;
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IESignaturesClient _eSignaturesClient;
    private readonly ILogger<UpdateDocumentStatusJob> _logger;

    public UpdateDocumentStatusJob(
        IMediator mediator,
        DocumentOnSAServiceDbContext dbContext,
        IESignaturesClient eSignaturesClient,
        ILogger<UpdateDocumentStatusJob> logger)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _eSignaturesClient = eSignaturesClient;
        _logger = logger;
    }

    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        var documentOnSas = await _dbContext.DocumentOnSa
            .Where(s => s.SignatureTypeId == SignatureTypes.Electronic.ToByte() && s.IsValid && !s.IsSigned && !s.IsFinal)
            .Select(s => new
            {
                s.DocumentOnSAId,
                s.ExternalId
            })
            .ToListAsync(cancellationToken);

        foreach (var documentOnSa in documentOnSas)
        {
            try
            {
                var status = await _eSignaturesClient.GetDocumentStatus(documentOnSa.ExternalId!, cancellationToken);

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
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}