using SharedTypes.Enums;
using SharedAudit;
using DomainServices.DocumentOnSAService.Api.Common;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using ExternalServices.ESignatures.V1;
using FastEnumUtility;
using Google.Protobuf.WellKnownTypes;
using Source = DomainServices.DocumentOnSAService.Api.Database.Enums.Source;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StopSigning;

public sealed class StopSigningHandler : IRequestHandler<StopSigningRequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IESignaturesClient _eSignaturesClient;
    private readonly IAuditLogger _auditLogger;
    private readonly ISalesArrangementStateManager _salesArrangementStateManager;
    private readonly ISalesArrangementServiceClient _salesArrangementServiceClient;

    public StopSigningHandler(
        DocumentOnSAServiceDbContext dbContext,
        IESignaturesClient eSignaturesClient,
        ISalesArrangementStateManager salesArrangementStateManager,
        ISalesArrangementServiceClient salesArrangementServiceClient,
        IAuditLogger auditLogger)

    {
        _dbContext = dbContext;
        _eSignaturesClient = eSignaturesClient;
        _auditLogger = auditLogger;
        _salesArrangementStateManager = salesArrangementStateManager;
        _salesArrangementServiceClient = salesArrangementServiceClient;
    }

    public async Task<Empty> Handle(StopSigningRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FindAsync(request.DocumentOnSAId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId);

        if (documentOnSa.SignatureTypeId == SignatureTypes.Electronic.ToByte() && request.NotifyESignatures) // 3
            await _eSignaturesClient.DeleteDocument(documentOnSa.ExternalId!, cancellationToken);

        documentOnSa.IsValid = false;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var salesArrangement = await _salesArrangementServiceClient.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);

        _auditLogger.Log(
            AuditEventTypes.Noby008,
            "Podepsaný dokument byl stornován",
            products: new List<AuditLoggerHeaderItem>
            {
                new(AuditConstants.ProductNamesCase, salesArrangement.CaseId),
                new(AuditConstants.ProductNamesSalesArrangement, documentOnSa.SalesArrangementId),
                new(AuditConstants.ProductNamesForm, documentOnSa.FormId),
            }
        );

        if (documentOnSa.Source == Source.Workflow)
            return new Empty();

        // SA state
        if (salesArrangement.State == (int)SalesArrangementStates.InSigning // 7
             || salesArrangement.State == (int)SalesArrangementStates.ToSend) // 8
        {
            await _salesArrangementStateManager.SetSalesArrangementStateAccordingDocumentsOnSa(salesArrangement.SalesArrangementId, cancellationToken);
        }
        else
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.SigningInvalidSalesArrangementState);
        }

        return new Empty();
    }
}
