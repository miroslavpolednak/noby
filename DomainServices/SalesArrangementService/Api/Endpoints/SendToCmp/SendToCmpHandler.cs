using CIS.Core;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using System.Globalization;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;

internal class SendToCmpHandler : IRequestHandler<SendToCmpRequest, Empty>
{
    private readonly ILogger<SendToCmpHandler> _logger;
    private readonly Database.NobyRepository _repository;
    private readonly UserService.Clients.IUserServiceClient _userService;
    private readonly IHouseholdServiceClient _householdClient;
    private readonly Services.Forms.FormsService _formsService;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICodebookServiceClients _codebookService;
    private readonly IDocumentGeneratorServiceClient _documentGeneratorService;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IMediator _mediator;

    public SendToCmpHandler(
        IMediator mediator,
        ILogger<SendToCmpHandler> logger,
        Database.NobyRepository repository,
        UserService.Clients.IUserServiceClient userService,
        IHouseholdServiceClient householdClient,
        Services.Forms.FormsService formsService,
        IDocumentOnSAServiceClient documentOnSAService,
        IDocumentArchiveServiceClient documentArchiveService,
        ICodebookServiceClients codebookService,
        IDocumentGeneratorServiceClient documentGeneratorService,
        IDateTime dateTime,
        ICurrentUserAccessor currentUserAccessor)
    {
        _mediator = mediator;
        _logger = logger;
        _repository = repository;
        _userService = userService;
        _householdClient = householdClient;
        _formsService = formsService;
        _documentOnSAService = documentOnSAService;
        _documentArchiveService = documentArchiveService;
        _codebookService = codebookService;
        _documentGeneratorService = documentGeneratorService;
        _dateTime = dateTime;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Empty> Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        var salesArrangement = await _formsService.LoadSalesArrangement(request.SalesArrangementId, cancellationToken);
        var category = await _formsService.LoadSalesArrangementCategory(salesArrangement, cancellationToken);

        var createdFinalVersionOfDocOnSa = new List<CreateDocumentOnSAResponse>();

        var easFormResponse = await GetEasForm(salesArrangement, category, createdFinalVersionOfDocOnSa, cancellationToken);

        await SaveDataSentence(easFormResponse, salesArrangement, createdFinalVersionOfDocOnSa, cancellationToken);

        //https://jira.kb.cz/browse/HFICH-4684 ToDo remove
        await _mediator.Send(new UpdateSalesArrangementStateRequest
        {
            SalesArrangementId = request.SalesArrangementId,
            State = (int)SalesArrangementStates.InApproval
        }, cancellationToken);

        return new Empty();
    }

    private async Task<GetEasFormResponse> GetEasForm(SalesArrangement salesArrangement, SalesArrangementCategories category, List<CreateDocumentOnSAResponse> createdFinalVersionOfDocOnSa, CancellationToken cancellationToken)
    {
        return category switch
        {
            SalesArrangementCategories.ProductRequest => await ProcessProductRequest(salesArrangement, createdFinalVersionOfDocOnSa, cancellationToken),
            SalesArrangementCategories.ServiceRequest => await ProcessServiceRequest(salesArrangement, createdFinalVersionOfDocOnSa, cancellationToken),
            _ => throw new NotImplementedException()
        };
    }

    private async Task<GetEasFormResponse> ProcessProductRequest(SalesArrangement salesArrangement, List<CreateDocumentOnSAResponse> createdFinalVersionOfDocOnSa, CancellationToken cancellationToken)
    {
        var dynamicValues = await CreateDynamicValuesWithFinalDocumentOnSa(salesArrangement, createdFinalVersionOfDocOnSa, cancellationToken);

        var response = await _formsService.LoadProductForm(salesArrangement, dynamicValues, cancellationToken);

        return response;
    }

    private async Task<GetEasFormResponse> ProcessServiceRequest(SalesArrangement salesArrangement, List<CreateDocumentOnSAResponse> createdFinalVersionOfDocOnSa, CancellationToken cancellationToken)
    {
        var dynamicValue = new DynamicFormValues();
        dynamicValue.DocumentId = await _documentArchiveService.GenerateDocumentId(new(), cancellationToken);
        dynamicValue.FormId = await _documentOnSAService.GenerateFormId(new() { IsFormIdFinal = true }, cancellationToken);
        dynamicValue.DocumentTypeId = await GetDocumentTypeIdForServiceRequest(salesArrangement, cancellationToken);
        
        createdFinalVersionOfDocOnSa.Add(await CreateFinalDocumentOnSa(salesArrangement, dynamicValue, cancellationToken));

        return await _formsService.LoadServiceForm(salesArrangement.SalesArrangementId, new List<DynamicFormValues>() { dynamicValue }, cancellationToken);
    }

    private async Task<List<DynamicFormValues>> CreateDynamicValuesWithFinalDocumentOnSa(SalesArrangement salesArrangement, List<CreateDocumentOnSAResponse> createdFinalVersionOfDocOnSa, CancellationToken cancellationToken)
    {
        var dynamicValues = new List<DynamicFormValues>();

        var houseHolds = await _householdClient.GetHouseholdList(salesArrangement.SalesArrangementId, cancellationToken);
        foreach (var household in houseHolds)
        {
            var dynamicValue = new DynamicFormValues();
            dynamicValue.HouseholdId = household.HouseholdId;
            dynamicValue.DocumentId = await _documentArchiveService.GenerateDocumentId(new(), cancellationToken);
            dynamicValue.FormId = await _documentOnSAService.GenerateFormId(new() { IsFormIdFinal = true }, cancellationToken);
            dynamicValue.DocumentTypeId = await GetDocumentType(household, cancellationToken);

            createdFinalVersionOfDocOnSa.Add(await CreateFinalDocumentOnSa(salesArrangement, dynamicValue, cancellationToken));

            dynamicValues.Add(dynamicValue);
        }

        return dynamicValues;
    }

    private async Task<int> GetDocumentTypeIdForServiceRequest(SalesArrangement salesArrangement, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);
        return documentTypes.Single(d => d.SalesArrangementTypeId == salesArrangement.SalesArrangementTypeId).Id;
    }

    private async Task<CreateDocumentOnSAResponse> CreateFinalDocumentOnSa(SalesArrangement salesArrangement, DynamicFormValues dynamicValue, CancellationToken cancellationToken)
    {
        return await _documentOnSAService.CreateDocumentOnSA(new()
        {
            SalesArrangementId = salesArrangement.SalesArrangementId,
            DocumentTypeId = dynamicValue.DocumentTypeId,
            FormId = dynamicValue.FormId,
            EArchivId = dynamicValue.DocumentId,
            IsFinal = true
        }, cancellationToken);
    }

    private async Task<int> GetDocumentType(Household household, CancellationToken cancellationToken)
    {
        var houseHoldType = await _codebookService.HouseholdTypes(cancellationToken);
        return houseHoldType.Single(r => r.Id == household.HouseholdTypeId).DocumentTypeId!.Value;
    }

    private async Task SaveDataSentence(GetEasFormResponse easFormResponse, Contracts.SalesArrangement salesArrangement, List<CreateDocumentOnSAResponse> createdFinalVersionOfDocOnSa, CancellationToken cancellationToken)
    {
        await SaveDataSentenceFormToEArchive(easFormResponse, salesArrangement, createdFinalVersionOfDocOnSa, cancellationToken);

        await SaveDataSentence(easFormResponse, salesArrangement, cancellationToken);
    }

    private async Task SaveDataSentenceFormToEArchive(GetEasFormResponse easFormResponse, SalesArrangement salesArrangement, List<CreateDocumentOnSAResponse> createdFinalVersionOfDocOnSa, CancellationToken cancellationToken)
    {
        foreach (var documentOnSaResponse in createdFinalVersionOfDocOnSa)
        {
            var generatedDocument = await GenerateDocument(salesArrangement, documentOnSaResponse, cancellationToken);

            await _documentArchiveService.UploadDocument(new()
            {
                BinaryData = generatedDocument.Data,
                SendDocumentOnly = false,
                Metadata = new DocumentArchiveService.Contracts.DocumentMetadata
                {
                    CaseId = salesArrangement.CaseId,
                    DocumentId = documentOnSaResponse.DocumentOnSa.EArchivId,
                    FormId = documentOnSaResponse.DocumentOnSa.FormId,
                    EaCodeMainId = int.Parse(easFormResponse.Forms
                    .Single(s => s.DynamicFormValues.DocumentId == documentOnSaResponse.DocumentOnSa.EArchivId).DefaultValues.PasswordCode,
                    CultureInfo.InvariantCulture),
                    Filename = await GetFileName(documentOnSaResponse.DocumentOnSa, cancellationToken),
                    CreatedOn = _dateTime.Now.Date,
                    ContractNumber = easFormResponse.ContractNumber,
                    AuthorUserLogin = _currentUserAccessor.User!.Id.ToString(CultureInfo.InvariantCulture)
                }
            }, cancellationToken);
        }
    }

    private async Task<Document> GenerateDocument(SalesArrangement salesArrangement, CreateDocumentOnSAResponse documentOnSaResponse, CancellationToken cancellationToken)
    {
        var documentOnSaData = await _documentOnSAService.GetDocumentOnSAData(documentOnSaResponse.DocumentOnSa.DocumentOnSAId!.Value, cancellationToken);
        var documentTemplateVersions = await _codebookService.DocumentTemplateVersions(cancellationToken);
        var documentTemplateVersion = documentTemplateVersions.Single(r => r.Id == documentOnSaData.DocumentTemplateVersionId).DocumentVersion;
        var generateDocumentRequest = CreateDocumentRequest(documentOnSaData, salesArrangement, documentTemplateVersion);
        var generatedDocument = await _documentGeneratorService.GenerateDocument(generateDocumentRequest, cancellationToken);
        return generatedDocument;
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

    private static GenerateDocumentRequest CreateDocumentRequest(GetDocumentOnSADataResponse documentOnSaData, SalesArrangement salesArrangement, string documentTemplateVersion)
    {
        var generateDocumentRequest = new GenerateDocumentRequest();
        generateDocumentRequest.DocumentTypeId = documentOnSaData.DocumentTypeId!.Value;
        generateDocumentRequest.DocumentTemplateVersion = documentTemplateVersion;
        generateDocumentRequest.OutputType = OutputFileType.Pdfa;
        generateDocumentRequest.Parts.Add(CreateDocPart(documentOnSaData, documentTemplateVersion));
        generateDocumentRequest.DocumentFooter = CreateFooter(salesArrangement);
        return generateDocumentRequest;
    }

    private static GenerateDocumentPart CreateDocPart(GetDocumentOnSADataResponse documentOnSaData, string documentTemplateVersion)
    {
        var docPart = new GenerateDocumentPart();
        docPart.DocumentTypeId = documentOnSaData.DocumentTypeId!.Value;
        docPart.DocumentTemplateVersion = documentTemplateVersion;
        var documentDataDtos = JsonConvert.DeserializeObject<List<DocumentDataDto>>(documentOnSaData.Data);
        docPart.Data.AddRange(CreateData(documentDataDtos));
        return docPart;
    }

    private static DocumentFooter CreateFooter(SalesArrangement salesArrangement)
    {
        return new DocumentFooter
        {
            SalesArrangementId = salesArrangement.SalesArrangementId
        };
    }

    private static IEnumerable<GenerateDocumentPartData> CreateData(List<DocumentDataDto>? documentDataDtos)
    {
        ArgumentNullException.ThrowIfNull(documentDataDtos);

        foreach (var documentDataDto in documentDataDtos)
        {
            var documentPartData = new GenerateDocumentPartData();
            documentPartData.Key = documentDataDto.FieldName;

            documentPartData.StringFormat = documentDataDto.StringFormat;

            switch (documentDataDto.ValueCase)
            {
                case 3:
                    documentPartData.Text = documentDataDto.Text ?? string.Empty;
                    break;
                case 4:
                    documentPartData.Date = new DateTime(documentDataDto.Date!.Year, documentDataDto.Date!.Month, documentDataDto.Date!.Day);
                    break;
                case 5:
                    documentPartData.Number = documentDataDto.Number;
                    break;
                case 6:
                    documentPartData.DecimalNumber = new GrpcDecimal(documentDataDto.DecimalNumber!.Units, documentDataDto.DecimalNumber!.Nanos);
                    break;
                case 7:
                    documentPartData.LogicalValue = documentDataDto.LogicalValue;
                    break;
                case 8:
                    throw new NotSupportedException("GenericTable is not supported");
                default:
                    throw new NotSupportedException("Notsupported oneof object");
            }

            yield return documentPartData;
        }
    }

    private async Task<string> GetFileName(DocumentOnSAToSign documentOnSa, CancellationToken cancellationToken)
    {
        var templates = await _codebookService.DocumentTypes(cancellationToken);
        var fileName = templates.First(t => t.Id == (int)documentOnSa.DocumentTypeId!).FileName;
        return $"{fileName}_{documentOnSa.DocumentOnSAId}_{_dateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture)}";
    }
}