using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Api.Database.DocumentArchiveService;
using DomainServices.SalesArrangementService.Api.Services.Forms;
using DomainServices.SalesArrangementService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;

internal sealed class SendToCmpHandler : IRequestHandler<SendToCmpRequest, Empty>
{
    private readonly FormsService _formsService;
    private readonly FormsDocumentService _formsDocumentService;
    private readonly IDocumentArchiveRepository _documentArchiveRepository;
    private readonly IMediator _mediator;

    public SendToCmpHandler(
        IMediator mediator,
        FormsService formsService,
        FormsDocumentService formsDocumentService,
        IDocumentArchiveRepository documentArchiveRepository)
    {
        _mediator = mediator;
        _formsService = formsService;
        _formsDocumentService = formsDocumentService;
        _documentArchiveRepository = documentArchiveRepository;
    }

    public async Task<Empty> Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _formsService.LoadSalesArrangement(request.SalesArrangementId, cancellationToken);
        var category = await _formsService.LoadSalesArrangementCategory(salesArrangement, cancellationToken);

        var easFormAndFinalDocOnSaData = await GetEasFormAndFinalDocOnSa(salesArrangement, category, cancellationToken);

        await SaveDataSentenceAndForm(easFormAndFinalDocOnSaData.easResponse, salesArrangement, easFormAndFinalDocOnSaData.finalVersionsOfDocOnSa, cancellationToken);

        //https://jira.kb.cz/browse/HFICH-4684 
        await _mediator.Send(new UpdateSalesArrangementStateRequest
        {
            SalesArrangementId = request.SalesArrangementId,
            State = (int)SalesArrangementStates.InApproval
        }, cancellationToken);

        return new Empty();
    }

    private async Task<(GetEasFormResponse easResponse, IReadOnlyCollection<CreateDocumentOnSAResponse> finalVersionsOfDocOnSa)> GetEasFormAndFinalDocOnSa(
        SalesArrangement salesArrangement,
        SalesArrangementCategories category,
        CancellationToken cancellationToken)
    {
        return category switch
        {
            SalesArrangementCategories.ProductRequest => await ProcessProductRequest(salesArrangement, cancellationToken),
            SalesArrangementCategories.ServiceRequest => await ProcessServiceRequest(salesArrangement, cancellationToken),
            _ => throw new NotImplementedException()
        };
    }

    private async Task<(GetEasFormResponse easResponse, IReadOnlyCollection<CreateDocumentOnSAResponse> finalVersionsOfDocOnSa)> ProcessProductRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicValues = await _formsService.CreateProductDynamicFormValues(salesArrangement, cancellationToken).ToListAsync(cancellationToken);

        var finalDocumentsOnSa = await Task.WhenAll(dynamicValues.Select(value => _formsDocumentService.CreateFinalDocumentOnSa(salesArrangement.SalesArrangementId, value, cancellationToken)));

        var easFormResponse = await _formsService.LoadProductForm(salesArrangement, dynamicValues, cancellationToken);

        return (easFormResponse, finalDocumentsOnSa);
    }

    private async Task<(GetEasFormResponse easResponse, IReadOnlyCollection<CreateDocumentOnSAResponse> finalVersionsOfDocOnSa)> ProcessServiceRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicValues = await _formsService.CreateServiceDynamicFormValues(salesArrangement, cancellationToken);

        var finalDocumentOnSa = await _formsDocumentService.CreateFinalDocumentOnSa(salesArrangement.SalesArrangementId, dynamicValues, cancellationToken);

        var formResponse = await _formsService.LoadServiceForm(salesArrangement.SalesArrangementId, new[] { dynamicValues }, cancellationToken);

        return (formResponse, new[] { finalDocumentOnSa });
    }

    private async Task SaveDataSentenceAndForm(GetEasFormResponse easFormResponse, SalesArrangement salesArrangement, IReadOnlyCollection<CreateDocumentOnSAResponse> createdFinalVersionOfDocOnSa, CancellationToken cancellationToken)
    {
        var entities = await _formsDocumentService.PrepareEntities(easFormResponse, salesArrangement, createdFinalVersionOfDocOnSa, cancellationToken);
        await _documentArchiveRepository.SaveDataSentenseWithForm(entities, cancellationToken);
    }
}