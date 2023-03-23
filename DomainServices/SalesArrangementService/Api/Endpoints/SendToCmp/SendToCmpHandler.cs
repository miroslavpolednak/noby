using CIS.Core;
using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Api.Services.Forms;
using DomainServices.SalesArrangementService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;

internal class SendToCmpHandler : IRequestHandler<SendToCmpRequest, Empty>
{
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Database.NobyRepository _repository;
    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly FormsService _formsService;
    private readonly FormsDocumentService _formsDocumentService;
    private readonly IDateTime _dateTime;
    private readonly IMediator _mediator;

    public SendToCmpHandler(
        IMediator mediator,
        ILogger<SendToCmpHandler> logger,
        Database.NobyRepository repository,
        UserService.Clients.IUserServiceClient userService,
        FormsService formsService,
        FormsDocumentService formsDocumentService,
        IDateTime dateTime)
    {
        _mediator = mediator;
        _logger = logger;
        _repository = repository;
        _userService = userService;
        _formsService = formsService;
        _formsDocumentService = formsDocumentService;
        _dateTime = dateTime;
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

    private async Task<(GetEasFormResponse easResponse, IEnumerable<CreateDocumentOnSAResponse> finalVersionsOfDocOnSa)> GetEasFormAndFinalDocOnSa(
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

    private async Task<(GetEasFormResponse easResponse, IEnumerable<CreateDocumentOnSAResponse> finalVersionsOfDocOnSa)> ProcessProductRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicValues = await _formsService.CreateProductDynamicFormValues(salesArrangement, cancellationToken).ToListAsync(cancellationToken);

        var finalDocumentsOnSa = await Task.WhenAll(dynamicValues.Select(value => _formsDocumentService.CreateFinalDocumentOnSa(salesArrangement.SalesArrangementId, value, cancellationToken)));

        var easFormResponse = await _formsService.LoadProductForm(salesArrangement, dynamicValues, cancellationToken);

        return (easFormResponse, finalDocumentsOnSa);
    }

    private async Task<(GetEasFormResponse easResponse, IEnumerable<CreateDocumentOnSAResponse> finalVersionsOfDocOnSa)> ProcessServiceRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var dynamicValues = await _formsService.CreateServiceDynamicFormValues(salesArrangement, cancellationToken);

        var finalDocumentOnSa = await _formsDocumentService.CreateFinalDocumentOnSa(salesArrangement.SalesArrangementId, dynamicValues, cancellationToken);

        var formResponse = await _formsService.LoadServiceForm(salesArrangement.SalesArrangementId, new[] { dynamicValues }, cancellationToken);

        return (formResponse, new[] { finalDocumentOnSa });
    }

    private async Task SaveDataSentenceAndForm(GetEasFormResponse easFormResponse, SalesArrangement salesArrangement, IEnumerable<CreateDocumentOnSAResponse> createdFinalVersionOfDocOnSa, CancellationToken cancellationToken)
    {
        await _formsDocumentService.SaveFormToEArchive(easFormResponse, salesArrangement, createdFinalVersionOfDocOnSa, cancellationToken);

        await SaveDataSentence(easFormResponse, salesArrangement, cancellationToken);
    }

    private async Task SaveDataSentence(GetEasFormResponse easFormResponse, SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        // load user
        var user = await _userService.GetUser(salesArrangement.Created.UserId!.Value, cancellationToken);

        // map to entities
        var entities = easFormResponse.Forms.Select(f => new Database.Entities.FormInstanceInterface
        {
            DOCUMENT_ID = f.DynamicFormValues!.DocumentId,
            FORM_TYPE = f.DefaultValues.FormType,
            FORM_KIND = 'N',
            CPM = user.CPM ?? string.Empty,
            ICP = user.ICP ?? string.Empty,
            CREATED_AT = _dateTime.Now, // what time zone?
            STORNO = 0,
            DATA_TYPE = 1,
            JSON_DATA_CLOB = f.Json
        }).ToList();

        await _repository.CreateForms(entities, cancellationToken);

        var documentIds = entities.Select(e => e.DOCUMENT_ID);

        _logger.LogInformation($"Entities {nameof(Database.Entities.FormInstanceInterface)} created [ContractNumber: {salesArrangement}, DokumentIds: {string.Join(", ", documentIds)} ]");
    }
}