using DomainServices.DocumentOnSAService.Clients;
using SharedAudit;
using DomainServices.SalesArrangementService.Api.Services.Forms;
using DomainServices.SalesArrangementService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;

internal sealed class SendToCmpHandler(
	IAuditLogger _auditLogger,
	FormsService _formsService,
	FormsDocumentService _formsDocumentService,
	PerformerProvider _performerProvider,
	IDocumentOnSAServiceClient _documentOnSAService)
		: IRequestHandler<SendToCmpRequest, Empty>
{
	public async Task<Empty> Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await LoadAndValidateSalesArrangement(request.SalesArrangementId, request.IsCancelled, cancellationToken);

        await ProcessEasFormIfNeeded(salesArrangement, request.IsCancelled, cancellationToken);

        if (!request.IsCancelled)
            await _formsService.UpdateSalesArrangementState(salesArrangement.SalesArrangementId, EnumSalesArrangementStates.InApproval, cancellationToken);

        AuditLog(salesArrangement);

        return new Empty();
    }

    private async Task<SalesArrangement> LoadAndValidateSalesArrangement(int salesArrangementId, bool isCancelled, CancellationToken cancellationToken)
    {
        if (!isCancelled)
            await _documentOnSAService.RefreshSalesArrangementState(salesArrangementId, cancellationToken);

        var salesArrangement = await _formsService.LoadSalesArrangement(salesArrangementId, cancellationToken);

        if (!isCancelled && salesArrangement.State != (int)EnumSalesArrangementStates.ToSend)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CannotBeSent, salesArrangement.State);

        return salesArrangement;
    }

    private async Task ProcessEasFormIfNeeded(SalesArrangement salesArrangement, bool isCancelled, CancellationToken cancellationToken)
    {
        var saType = await _formsService.LoadSalesArrangementType(salesArrangement.SalesArrangementTypeId, cancellationToken);

        if (!saType.IsFormSentToCmp)
            return;

        switch ((SalesArrangementCategories)saType.SalesArrangementCategory)
        {
            case SalesArrangementCategories.ProductRequest:
                await ProcessProductRequest(salesArrangement, isCancelled, cancellationToken);
                break;

            case SalesArrangementCategories.ServiceRequest:
                await ProcessServiceRequest(salesArrangement, cancellationToken);
                break;

            case SalesArrangementCategories.Unknown:
            default:
                throw new NotImplementedException();
        }
    }

    private async Task ProcessProductRequest(SalesArrangement salesArrangement, bool isCancelled, CancellationToken cancellationToken)
    {
        var dynamicFormValues = await _formsService.CreateProductDynamicFormValues(salesArrangement, cancellationToken).ToArrayAsync(cancellationToken);

        await _performerProvider.SetDynamicValuesPerformerUserId(salesArrangement.CaseId, dynamicFormValues, cancellationToken);

        var finalDocumentsOnSa = await _formsDocumentService.CreateFinalDocumentsOnSa(salesArrangement.SalesArrangementId, cancellationToken, dynamicFormValues).ToListAsync(cancellationToken);

        var easFormResponse = await _formsService.LoadProductForm(salesArrangement, dynamicFormValues, isCancelled, cancellationToken);

        await _formsDocumentService.SaveEasForms(easFormResponse, salesArrangement, finalDocumentsOnSa, cancellationToken);
    }

    private async Task ProcessServiceRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicFormValues = await _formsService.CreateServiceDynamicFormValues(salesArrangement, cancellationToken);

        if (salesArrangement.SalesArrangementTypeId == (int)SalesArrangementTypes.Drawing)
        {
            var easFormResponse = await _formsService.LoadServiceForm(salesArrangement, dynamicFormValues, cancellationToken);

            await _formsDocumentService.SaveEasForms(easFormResponse, salesArrangement, cancellationToken);
        }
        else
        {
            var finalDocumentOnSa = await _formsDocumentService.CreateFinalDocumentsOnSa(salesArrangement.SalesArrangementId, cancellationToken, dynamicFormValues).ToArrayAsync(cancellationToken);

            var easFormResponse = await _formsService.LoadServiceForm(salesArrangement, dynamicFormValues, cancellationToken);

            await _formsDocumentService.SaveEasForms(easFormResponse, salesArrangement, finalDocumentOnSa, cancellationToken);
        }
    }

    private void AuditLog(SalesArrangement salesArrangement)
    {
        var products = new List<AuditLoggerHeaderItem>
        {
            new(AuditConstants.ProductNamesCase, salesArrangement.CaseId),
            new(AuditConstants.ProductNamesSalesArrangement, salesArrangement.SalesArrangementId)
        };

        _auditLogger.Log(AuditEventTypes.Noby005, message: "Žádost byla dokončena", products: products);
    }
}