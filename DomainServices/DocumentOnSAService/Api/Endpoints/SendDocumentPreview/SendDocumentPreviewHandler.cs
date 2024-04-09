using SharedTypes.Enums;
using SharedAudit;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.ESignatures.V1;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SendDocumentPreview;

public class SendDocumentPreviewHandler : IRequestHandler<SendDocumentPreviewRequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IESignaturesClient _signaturesClient;
    private readonly IAuditLogger _auditLogger;

    public async Task<Empty> Handle(SendDocumentPreviewRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa
            .Where(r => r.DocumentOnSAId == request.DocumentOnSAId)
            .FirstOrDefaultAsync(cancellationToken)
        ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId);

        if (documentOnSa.SignatureTypeId != (int)SignatureTypes.Electronic)
        {
            throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.UnableToSendDocumentPreviewForPaperSignedDocuments);
        }

        var (code, message) = await _signaturesClient.SendDocumentPreview(documentOnSa.ExternalIdESignatures ?? string.Empty, cancellationToken);

        documentOnSa.IsPreviewSentToCustomer = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _auditLogger.Log(
            AuditEventTypes.Noby011,
            "Dokument byl odeslán klientovi k náhledu",
            products: new List<AuditLoggerHeaderItem>
            {
                // new("case", todo),
                new(AuditConstants.ProductNamesSalesArrangement, documentOnSa.SalesArrangementId),
                new(AuditConstants.ProductNamesForm, documentOnSa.FormId)
            }
        );

        return new Empty();
    }

    public SendDocumentPreviewHandler(
        DocumentOnSAServiceDbContext dbContext,
        IESignaturesClient signaturesClient,
        IAuditLogger auditLogger)
    {
        _dbContext = dbContext;
        _signaturesClient = signaturesClient;
        _auditLogger = auditLogger;
    }
}