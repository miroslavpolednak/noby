using CIS.Foms.Enums;
using CIS.Infrastructure.Audit;
using DomainServices.SalesArrangementService.Api.Services.Forms;
using DomainServices.SalesArrangementService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;

internal sealed class SendToCmpHandler : IRequestHandler<SendToCmpRequest, Empty>
{
    private readonly FormsService _formsService;
    private readonly FormsDocumentService _formsDocumentService;
    private readonly PerformerProvider _performerProvider;
    private readonly IMediator _mediator;
    private readonly IAuditLogger _auditLogger;

    public SendToCmpHandler(
        IAuditLogger auditLogger,
        IMediator mediator,
        FormsService formsService,
        FormsDocumentService formsDocumentService,
        PerformerProvider performerProvider)
    {
        _auditLogger = auditLogger;
        _mediator = mediator;
        _formsService = formsService;
        _formsDocumentService = formsDocumentService;
        _performerProvider = performerProvider;
    }

    public async Task<Empty> Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _formsService.LoadSalesArrangement(request.SalesArrangementId, cancellationToken);
        var category = await _formsService.LoadSalesArrangementCategory(salesArrangement, cancellationToken);

        //TODO: Mock - what to do when a service SA does not have DV
        if (salesArrangement.SalesArrangementTypeId is not (7 or 8 or 9))
        {
            await ProcessEasForm(salesArrangement, category, cancellationToken);
        }

        //https://jira.kb.cz/browse/HFICH-4684 
        await _mediator.Send(new UpdateSalesArrangementStateRequest
        {
            SalesArrangementId = request.SalesArrangementId,
            State = (int)SalesArrangementStates.InApproval
        }, cancellationToken);

        // auditni log
        _auditLogger.LogWithCurrentUser(
            AuditEventTypes.Noby005,
            "Žádost byla dokončena",
            products: new List<AuditLoggerHeaderItem>()
            {
                new("case", salesArrangement.CaseId),
                new("salesArrangement", request.SalesArrangementId)
            }
        );

        return new Empty();
    }
    private Task ProcessEasForm(SalesArrangement salesArrangement, SalesArrangementCategories category, CancellationToken cancellationToken) =>
        category switch
        {
            SalesArrangementCategories.ProductRequest => ProcessProductRequest(salesArrangement, cancellationToken),
            SalesArrangementCategories.ServiceRequest => ProcessServiceRequest(salesArrangement, cancellationToken),
            _ => throw new NotImplementedException()
        };

    private async Task ProcessProductRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicValues = await _formsService.CreateProductDynamicFormValues(salesArrangement, cancellationToken).ToListAsync(cancellationToken);

        var finalDocumentsOnSa = await Task.WhenAll(dynamicValues.Select(value => _formsDocumentService.CreateFinalDocumentOnSa(salesArrangement.SalesArrangementId, value, cancellationToken)));

        dynamicValues.First(v => v.DocumentTypeId == (int)DocumentTypes.ZADOSTHU).PerformerUserId = await _performerProvider.LoadPerformerUserId(salesArrangement.CaseId, cancellationToken);

        var easFormResponse = await _formsService.LoadProductForm(salesArrangement, dynamicValues, cancellationToken);

        await _formsDocumentService.SaveEasForms(easFormResponse, salesArrangement, finalDocumentsOnSa, cancellationToken);
    }

    private async Task ProcessServiceRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicValues = await _formsService.CreateServiceDynamicFormValues(salesArrangement, cancellationToken);

        var finalDocumentOnSa = await _formsDocumentService.CreateFinalDocumentOnSa(salesArrangement.SalesArrangementId, dynamicValues, cancellationToken);

        var easFormResponse = await _formsService.LoadServiceForm(salesArrangement, new[] { dynamicValues }, cancellationToken);

        await _formsDocumentService.SaveEasForms(easFormResponse, salesArrangement, new[] { finalDocumentOnSa }, cancellationToken);
    }
}