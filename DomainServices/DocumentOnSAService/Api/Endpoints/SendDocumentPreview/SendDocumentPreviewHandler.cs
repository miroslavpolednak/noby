using CIS.Foms.Enums;
using CIS.Infrastructure.Audit;
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
        
        var (code, message) = await _signaturesClient.SendDocumentPreview(documentOnSa.ExternalId ?? string.Empty, cancellationToken);
        
        _auditLogger.LogWithCurrentUser(
            AuditEventTypes.Noby011,
            "Náhled dokumentu byl odeslán klientovi.",
            products: new List<AuditLoggerHeaderItem>
            {
                new("documentOnSA", documentOnSa.DocumentOnSAId)
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