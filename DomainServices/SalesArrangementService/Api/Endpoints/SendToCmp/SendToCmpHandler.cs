using DomainServices.DocumentOnSAService.Clients;
using SharedAudit;
using DomainServices.SalesArrangementService.Api.Services.Forms;
using DomainServices.SalesArrangementService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;

internal sealed class SendToCmpHandler : IRequestHandler<SendToCmpRequest, Empty>
{
    private readonly FormsService _formsService;
    private readonly FormsDocumentService _formsDocumentService;
    private readonly PerformerProvider _performerProvider;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IAuditLogger _auditLogger;

    public SendToCmpHandler(
        IAuditLogger auditLogger,
        FormsService formsService,
        FormsDocumentService formsDocumentService,
        PerformerProvider performerProvider,
        IDocumentOnSAServiceClient documentOnSAService)
    {
        _auditLogger = auditLogger;
        _formsService = formsService;
        _formsDocumentService = formsDocumentService;
        _performerProvider = performerProvider;
        _documentOnSAService = documentOnSAService;
    }

    public async Task<Empty> Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _formsService.LoadSalesArrangement(request.SalesArrangementId, cancellationToken);
        var saType = await _formsService.LoadSalesArrangementType(salesArrangement.SalesArrangementTypeId, cancellationToken);

        if (saType.IsFormSentToCmp)
        {
            salesArrangement = await ValidateAndReloadSalesArrangement(salesArrangement.SalesArrangementId, cancellationToken);

            await ProcessEasForm(salesArrangement, (SalesArrangementCategories)saType.SalesArrangementCategory, request.IsCancelled, cancellationToken);
        }
        
        await _formsService.UpdateSalesArrangementState(salesArrangement.SalesArrangementId, SalesArrangementStates.InApproval, cancellationToken);

        AuditLog(salesArrangement);

        return new Empty();
    }

    private async Task<SalesArrangement> ValidateAndReloadSalesArrangement(int salesArrangementId, CancellationToken cancellationToken)
    {
        await _documentOnSAService.RefreshSalesArrangementState(salesArrangementId, cancellationToken);

        var salesArrangement = await _formsService.LoadSalesArrangement(salesArrangementId, cancellationToken);

        if (salesArrangement.State != (int)SalesArrangementStates.ToSend)
            throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.CannotBeSent, salesArrangement.State);

        return salesArrangement;
    }

    private async Task ProcessEasForm(SalesArrangement salesArrangement, SalesArrangementCategories salesArrangementCategory, bool isCancelled, CancellationToken cancellationToken)
    {
        switch (salesArrangementCategory)
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