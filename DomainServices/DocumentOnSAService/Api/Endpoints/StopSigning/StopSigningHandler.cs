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

public sealed class StopSigningHandler(
	DocumentOnSAServiceDbContext _dbContext,
	IESignaturesClient _eSignaturesClient,
	ISalesArrangementStateManager _salesArrangementStateManager,
	ISalesArrangementServiceClient _salesArrangementServiceClient,
	IAuditLogger _auditLogger,
	ILogger<StopSigningHandler> _logger) 
    : IRequestHandler<StopSigningRequest, Empty>
{
    private readonly static EnumSalesArrangementStates[] _enumSalesArrangementStates =
    [
        EnumSalesArrangementStates.InProgress,
		EnumSalesArrangementStates.InApproval,
		EnumSalesArrangementStates.Cancelled,
		EnumSalesArrangementStates.NewArrangement,
		EnumSalesArrangementStates.Disbursed,
		EnumSalesArrangementStates.Finished,
		EnumSalesArrangementStates.RC2
	];

	public async Task<Empty> Handle(StopSigningRequest request, CancellationToken cancellationToken)
    {
        var documentOnSa = await _dbContext.DocumentOnSa.FindAsync([request.DocumentOnSAId, cancellationToken], cancellationToken: cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist, request.DocumentOnSAId);

        var salesArrangement = await _salesArrangementServiceClient.GetSalesArrangement(documentOnSa.SalesArrangementId, cancellationToken);

        if (!request.SkipValidations && salesArrangement.IsInState(_enumSalesArrangementStates))
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
                    products:
					[
				        new(AuditConstants.ProductNamesCase, salesArrangement.CaseId),
                        new(AuditConstants.ProductNamesSalesArrangement, documentOnSa.SalesArrangementId),
                        new(AuditConstants.ProductNamesForm, documentOnSa.FormId),
                    ]
                );

        if (documentOnSa.Source != Source.Workflow)
		{
            // SA state
			await _salesArrangementStateManager.SetSalesArrangementStateAccordingDocumentsOnSa(salesArrangement.SalesArrangementId, cancellationToken);
		}
        
        return new Empty();
    }
}
