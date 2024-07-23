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
using DomainServices.DocumentOnSAService.Api.Extensions;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.StopSigning;

public sealed class StopSigningHandler : IRequestHandler<StopSigningRequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _dbContext;
    private readonly IESignaturesClient _eSignaturesClient;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<StopSigningHandler> _logger;
    private readonly ISalesArrangementStateManager _salesArrangementStateManager;
    private readonly ISalesArrangementServiceClient _salesArrangementServiceClient;

    public StopSigningHandler(
        DocumentOnSAServiceDbContext dbContext,
        IESignaturesClient eSignaturesClient,
        ISalesArrangementStateManager salesArrangementStateManager,
        ISalesArrangementServiceClient salesArrangementServiceClient,
        IAuditLogger auditLogger,
        ILogger<StopSigningHandler> logger)

    {
        _dbContext = dbContext;
        _eSignaturesClient = eSignaturesClient;
        _auditLogger = auditLogger;
        _logger = logger;
        _salesArrangementStateManager = salesArrangementStateManager;
        _salesArrangementServiceClient = salesArrangementServiceClient;
    }

    public async Task<Empty> Handle(StopSigningRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FindAsync([request.DocumentOnSAId, cancellationToken], cancellationToken: cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId);

        var salesArrangement = await _salesArrangementServiceClient.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);

        if (salesArrangement.State != (int)EnumSalesArrangementStates.InSigning // 7
            && salesArrangement.State != (int)EnumSalesArrangementStates.ToSend // 8
            && !request.SkipValidations)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.SigningInvalidSalesArrangementState);
        }

        if (documentOnSa.SignatureTypeId == SignatureTypes.Electronic.ToByte() && request.NotifyESignatures) // 3
        {
            try
            {
                await _eSignaturesClient.DeleteDocument(documentOnSa.ExternalIdESignatures!, cancellationToken);
            }
            catch (Exception exp)
            {
                _logger.StopSigningError(documentOnSa.DocumentOnSAId, exp);
            }
        }

        documentOnSa.IsValid = false;

        await _dbContext.SaveChangesAsync(cancellationToken);

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
        await _salesArrangementStateManager.SetSalesArrangementStateAccordingDocumentsOnSa(salesArrangement.SalesArrangementId, cancellationToken);

        return new Empty();
    }
}
